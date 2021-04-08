using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContructBigControl : MonoBehaviour
{


    public Text textDisDes;
    int curTurn = 1;
    bool duringTrans = true;
    public Animator animations;
    public TextMeshProUGUI bigMazeText;

    public List<GameObject> overButton;

    public Transform bigMaze;
    GameObject[,] tileMaze;
    GameObject[,] wallMaze;
    int rowMaze;
    int columnMaze;
    float fieldWidth;
    float fieldHeight;

    public Text Debuggg;
    public PauseManager pauseBruh;

    const float heightPercent = 0.1f;//Put your magic number here :)
    const float hitBoxPercent = 0.2f;
    float scaleMaze = 1f;

    public Color[] mainColor = new Color[2];

    MazeManager thisMaze;

    short[,] isWallCon;
    bool mouseOffReset = false;

    int phase = 0;
    /* 0 is wait
     * 1 is Contruct
     * 2 is sure ?
    */



    void Start()
    {

        Sprite picBgField = Resources.Load<Sprite>("Texture/MazeField/BgField");
        fieldWidth = picBgField.rect.width / picBgField.pixelsPerUnit;
        fieldHeight = picBgField.rect.height / picBgField.pixelsPerUnit;
        float wallZoom = picBgField.rect.width;
        wallZoom /= Resources.Load<Sprite>("Texture/MazeField/CornerZ_Line").rect.width;

        if (GameDataManager.getPhase() == "ConP1")
        {
            curTurn = 1;
            thisMaze = GameDataManager.player1Maze;
        }
        else
        {
            curTurn = 2;
            thisMaze = GameDataManager.player2Maze;
        }

        textDisDes.text = string.Format(LangManager.calling("CDisDes"), curTurn, curTurn == 1 ? 2 : 1);
        bigMazeText.text = string.Format(LangManager.calling("CMaze"), curTurn);

        rowMaze = GameDataManager.getRowMaze();
        columnMaze = GameDataManager.getColumnMaze();

        tileMaze = new GameObject[rowMaze, columnMaze];
        wallMaze = new GameObject[rowMaze + 1, columnMaze + 1];
        isWallCon = new short[rowMaze, columnMaze];

        bigMaze.localPosition = new Vector3(fieldWidth / 2 - fieldWidth * columnMaze / 2,
            fieldHeight / 2 - fieldHeight * rowMaze / 2);


        for (int i = 0; i < rowMaze; i++)
        {
            for (int j = 0; j < columnMaze; j++)
            {
                tileMaze[i, j] = new GameObject(
                    string.Format("tile[{0},{1}]", i, j));
                tileMaze[i, j].transform.parent = bigMaze;
                tileMaze[i, j].AddComponent<SpriteRenderer>();
                tileMaze[i, j].GetComponent<SpriteRenderer>().sprite = picBgField;
                tileMaze[i, j].transform.localPosition = new Vector3(j * fieldWidth, i * fieldHeight);
            }
        }

        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                wallMaze[i, j] = new GameObject(
                    string.Format("wall[{0},{1}]", i, j));
                wallMaze[i, j].transform.parent = bigMaze;
                wallMaze[i, j].AddComponent<SpriteRenderer>();
                wallMaze[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerE_Line");
                wallMaze[i, j].GetComponent<SpriteRenderer>().color = mainColor[curTurn - 1];
                wallMaze[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBorder";
                wallMaze[i, j].transform.localPosition = new Vector3((j - 0.5f) * fieldWidth, (rowMaze - i - 0.5f) * fieldHeight);
                wallMaze[i, j].transform.localScale = new Vector3(wallZoom, wallZoom, 1f);

            }
        }
        reloadWalls();

        StartCoroutine(waitTran(1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(!duringTrans)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (phase == 0)
                {
                    phase = 1;
                    animations.SetTrigger("WarnEnter");
                    StartCoroutine(inActiveThem());
                }
                if (phase == 2)
                {
                    phase = 3;
                    animations.SetTrigger("Proceed");
                    
                    StartCoroutine(nextBruh());
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && phase == 1)
            {
                pauseBruh.callPause();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && phase == 2)
            {
                animations.SetTrigger("BackEdit");
                StartCoroutine(waitTran(1f));
                phase = 1;
                StartCoroutine(inActiveThem());
            }
           
            if (phase == 1 && Input.GetMouseButton(0) )
            {
                mouseOffReset = false;
                (int,int,char) result = fromPos(Input.mousePosition);
                int tr = result.Item1;
                int tc = result.Item2;
                if (result.Item3 != '?')
                {
                    if (true) //if mode is construct walls
                    {
                        bool isNew = false;
                        switch (result.Item3)
                        {
                            case 'U':
                                if ((isWallCon[tr, tc] & 1) == 0)
                                {
                                    isWallCon[tr, tc] |= 1;
                                    if (tr - 1 >= 0) isWallCon[tr - 1, tc] |= 4;
                                    isNew = true;
                                }
                                break;
                            case 'R':
                                if ((isWallCon[tr, tc] & 2) == 0)
                                {
                                    isWallCon[tr, tc] |= 2;
                                    if (tc + 1 < columnMaze) isWallCon[tr, tc + 1] |= 8;
                                    isNew = true;
                                }
                                break;
                            case 'D':
                                if ((isWallCon[tr, tc] & 4) == 0)
                                {
                                    isWallCon[tr, tc] |= 4;
                                    if (tr + 1 < rowMaze) isWallCon[tr + 1, tc] |= 1;
                                    isNew = true;
                                }
                                break;
                            case 'L':
                                if ((isWallCon[tr, tc] & 8) == 0)
                                {
                                    isWallCon[tr, tc] |= 8;
                                    if (tc - 1 >= 0) isWallCon[tr, tc - 1] |= 2;
                                    isNew = true;
                                }
                                break;
                        }

                        if (isNew)
                        {
                            thisMaze.toggleWall(tr, tc, result.Item3);
                            reloadWalls();
                        }
                    }
                }
            }
            else if (!mouseOffReset)
            {
                for(int i = 0; i < rowMaze; i++)
                {
                    for(int j = 0; j < columnMaze; j++)
                    {
                        isWallCon[i, j] = 0;
                    }
                }
                mouseOffReset = true;
            }
        }
    }

    IEnumerator waitTran(float halt)
    {
        duringTrans = true;
        yield return new WaitForSeconds(halt);
        duringTrans = false;
    }

    IEnumerator inActiveThem()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject e in overButton)
        {
            e.SetActive(false);
        }
    }

    IEnumerator nextBruh()
    {
        duringTrans = true;
        yield return new WaitForSeconds(3f);

        if (curTurn == 1)
        {
            GameDataManager.setPhase("ConP2");
            GameDataManager.saveGame();
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.ContructMaze);
        }
        else
        {
            GameDataManager.setPhase("Solving");
            GameDataManager.saveGame();
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.SolveMaze);
        }

        
    }

    public (int,int,char) fromPos(Vector3 mousePos)
    {

        float realWidthPercent = heightPercent * Screen.height / Screen.width;


        float mazeWidth = columnMaze * realWidthPercent * scaleMaze * Screen.width;
        float mazeHeight = rowMaze * heightPercent * scaleMaze * Screen.height;

        Vector2 ULMaze = new Vector2(
            (Screen.width - mazeWidth) / 2f,
            Screen.height - (Screen.height - mazeHeight) / 2f);

        Vector2 DRMaze = new Vector2(
            Screen.width - (Screen.width - mazeWidth) / 2f,
             (Screen.height - mazeHeight) / 2f);


        if(mousePos.x >= ULMaze.x && mousePos.x <= DRMaze.x &&
            mousePos.y <= ULMaze.y && mousePos.y >= DRMaze.y)
        {

            float tileX = (mousePos.x - ULMaze.x) * columnMaze / mazeWidth;
            float tileY = rowMaze - (mousePos.y - DRMaze.y) * rowMaze / mazeHeight;

            short dirCode = 0;

            if (tileY % 1f <= hitBoxPercent) dirCode |= 1;
            if (tileX % 1f >= 1f - hitBoxPercent) dirCode |= 2;
            if (tileY % 1f >= 1f - hitBoxPercent) dirCode |= 4;
            if (tileX % 1f <= hitBoxPercent) dirCode |= 8;

            char dir = '?';
            if (dirCode == 1) dir = 'U';
            else if (dirCode == 2) dir = 'R';
            else if (dirCode == 4) dir = 'D';
            else if (dirCode == 8) dir = 'L';


            Debuggg.text = string.Format("InMaze\n<{0},{1} dir {2}>", (int)tileY, (int)tileX, dir);
            
            if (((int)tileY == 0 && dir == 'U') || ((int)tileY == rowMaze - 1 && dir == 'D')
                || ((int)tileX == 0 && dir == 'L') || ((int)tileX == columnMaze - 1 && dir == 'R'))
            {
                Debuggg.text = string.Format("InMaze\nNOPE <{0},{1} dir {2}>", (int)tileY, (int)tileX, dir);
                return (-1, -1, '?');
            }

                return ((int)tileY, (int)tileX, dir);
        }
        Debuggg.text = "NOT In Maze";

        return (-1, -1, '?');
    }


    private (string, short) wallShortToIt(short typeS)
    {
        switch (typeS)
        {
            case 1: return ("A", 1);
            case 2: return ("A", 0);
            case 4: return ("A", 3);
            case 8: return ("A", 2);
            case 6: return ("B", 0);
            case 12: return ("B", 3);
            case 9: return ("B", 2);
            case 3: return ("B", 1);
            case 5: return ("C", 1);
            case 10: return ("C", 0);
            case 11: return ("D", 0);
            case 7: return ("D", 3);
            case 14: return ("D", 2);
            case 13: return ("D", 1);
            case 15: return ("E", 0);
            default: return ("Z", 0);
        }
    }

    private void reloadWalls()
    {
        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                short state = 0;

                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (thisMaze.isPass(i - 1, j - 1, 'R') == false)
                    {
                        state |= 1;
                    }
                    if (thisMaze.isPass(i - 1, j - 1, 'D') == false)
                    {
                        state |= 8;
                    }
                }
                if (i < rowMaze && j < columnMaze)
                {
                    if (thisMaze.isPass(i, j, 'U') == false)
                    {
                        state |= 2;
                    }
                    if (thisMaze.isPass(i, j, 'L') == false)
                    {
                        state |= 4;
                    }
                }
                if (i - 1 >= 0 && j < columnMaze)
                {
                    if (thisMaze.isPass(i - 1, j, 'D') == false)
                    {
                        state |= 2;
                    }
                    if (thisMaze.isPass(i - 1, j, 'L') == false)
                    {
                        state |= 1;
                    }
                }
                if (i < rowMaze && j - 1 >= 0)
                {
                    if (thisMaze.isPass(i, j - 1, 'R') == false)
                    {
                        state |= 4;
                    }
                    if (thisMaze.isPass(i, j - 1, 'U') == false)
                    {
                        state |= 8;
                    }
                }

                (string, short) tran = wallShortToIt(state);
                wallMaze[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Corner" + tran.Item1 + "_Line");
                wallMaze[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * tran.Item2);

                //borderLight[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Corner" + tran.Item1 + "_Light");
                //borderLight[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * tran.Item2);

            }
        }
    }

    public void subMaze()
    {
        //Calculate maze here
        if (true)
        {
            foreach (GameObject e in overButton)
            {
                e.SetActive(true);
            }
            animations.SetTrigger("Submit");
            phase = 2;
        }
    }

}
