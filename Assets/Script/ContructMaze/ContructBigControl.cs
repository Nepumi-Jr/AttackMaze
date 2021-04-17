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
    GameObject[,] wallLMaze;
    int rowMaze;
    int columnMaze;
    float fieldWidth;
    float fieldHeight;

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

    public GameObject mazeButtonsPanel;
    public Button[] toolsButtons = new Button[4];
    public Color NormalButtonColor;
    public Color HoverButtonColor;
    public Color ActivateColor;

    GameObject flgObject;
    GameObject staObject;

    short tool = 0;
    /* 0 is wall
     * 1 is Erase
     * 2 is setStart
     * 3 is setEnd
    */

    public GameObject DiffText;
    public Button subButton;
    public BGMManager mainBGM;
    public float lightWallOpa;
    public SfxList sfxManager;

    Vector3 posMazeToV3(float row, float column)
    {
        return new Vector3(column * fieldWidth + fieldWidth / 2 - fieldWidth * columnMaze / 2,
            (rowMaze - row - 1f) * fieldHeight + fieldHeight / 2 - fieldHeight * rowMaze / 2);
    }

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

        textDisDes.font = LangManager.textFont;
        textDisDes.text = string.Format(LangManager.calling("CDisDes"), curTurn, curTurn == 1 ? 2 : 1);

        bigMazeText.font = TMP_FontAsset.CreateFontAsset(LangManager.titleTextFont);
        bigMazeText.text = string.Format(LangManager.calling("CMaze"), curTurn);

        rowMaze = GameDataManager.getRowMaze();
        columnMaze = GameDataManager.getColumnMaze();

        tileMaze = new GameObject[rowMaze, columnMaze];
        wallMaze = new GameObject[rowMaze + 1, columnMaze + 1];
        wallLMaze = new GameObject[rowMaze + 1, columnMaze + 1];
        isWallCon = new short[rowMaze, columnMaze];

        scaleMaze = Mathf.Min(7f / rowMaze,1f);




        bigMaze.localPosition = new Vector3(0f,-1f);
        bigMaze.localScale = new Vector3(scaleMaze, scaleMaze);

        DiffText.transform.parent = bigMaze.transform;
        DiffText.transform.localPosition = new Vector3(columnMaze * fieldWidth / 2,
            rowMaze * fieldHeight / 2+0.2f);
        DiffText.transform.localScale = new Vector3(0.1f,0.1f);


        for (int i = 0; i < rowMaze; i++)
        {
            for (int j = 0; j < columnMaze; j++)
            {
                tileMaze[i, j] = new GameObject(
                    string.Format("tile[{0},{1}]", i, j));
                tileMaze[i, j].transform.parent = bigMaze;
                tileMaze[i, j].AddComponent<SpriteRenderer>();
                tileMaze[i, j].GetComponent<SpriteRenderer>().sprite = picBgField;
                tileMaze[i, j].transform.localPosition = posMazeToV3(i, j);
                tileMaze[i, j].transform.localScale = new Vector3(1f, 1f);
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
                wallMaze[i, j].transform.localPosition = posMazeToV3(i - 0.5f, j - 0.5f);
                wallMaze[i, j].transform.localScale = new Vector3(wallZoom, wallZoom, 1f);

                wallLMaze[i, j] = new GameObject(
                    string.Format("wallL[{0},{1}]", i, j));
                wallLMaze[i, j].transform.parent = bigMaze;
                wallLMaze[i, j].AddComponent<SpriteRenderer>();
                wallLMaze[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerE_Line");
                wallLMaze[i, j].GetComponent<SpriteRenderer>().color = mainColor[curTurn - 1];
                wallLMaze[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBorder";
                wallLMaze[i, j].transform.localPosition = posMazeToV3(i - 0.5f, j - 0.5f);
                wallLMaze[i, j].transform.localScale = new Vector3(wallZoom, wallZoom, 1f);

            }
        }
        flgObject = new GameObject("FLG");
        flgObject.transform.parent = bigMaze;
        flgObject.AddComponent<SpriteRenderer>();
        flgObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Finish");
        flgObject.GetComponent<SpriteRenderer>().sortingLayerName = "MazeFlag";

        staObject = new GameObject("STA");
        staObject.transform.parent = bigMaze;
        staObject.AddComponent<SpriteRenderer>();
        staObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/Icons/Start");
        staObject.GetComponent<SpriteRenderer>().sortingLayerName = "MazeFlag";

        reloadStartEnd();
        reloadWalls();
        changeTool(0);

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
                    mainBGM.startPlaying();
                    phase = 1;
                    animations.SetTrigger("WarnEnter");
                    sfxManager.playSfx("Start");
                    StartCoroutine(inActiveThem());
                }
                if (phase == 2)
                {
                    mainBGM.fadeVolume(0f, 1.2f);
                    phase = 3;
                    animations.SetTrigger("Proceed");
                    sfxManager.playSfx("Start");

                    StartCoroutine(nextBruh());
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && phase == 1)
            {
                sfxManager.playSfx("Click");
                pauseBruh.callPause();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && phase == 2)
            {
                animations.SetTrigger("BackEdit");
                sfxManager.playSfx("Click");
                mazeButtonsPanel.SetActive(true);
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
                if ((tool == 0 || tool == 1 ) && result.Item3 != '?')
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
                            if((tool == 1 && !thisMaze.isPass(tr, tc, result.Item3)) || tool == 0)
                            {
                                sfxManager.playSfx("WallPlace");
                                thisMaze.toggleWall(tr, tc, result.Item3);
                                reloadWalls();
                            }
                        }
                    }
                }
                
            }
            if(phase == 1 && Input.GetMouseButtonDown(0))
            {
                (int, int, char) result = fromPos(Input.mousePosition);
                int tr = result.Item1;
                int tc = result.Item2;
                if ((tool == 2 || tool == 3) && tr >= 0 && tc >= 0)
                {
                    if(tool == 2)
                    {
                        sfxManager.playSfx("WallPlace");
                        if (thisMaze.getEnd().x == tr && thisMaze.getEnd().y == tc)
                        {
                            thisMaze.setEnd(-1, -1);
                        }
                        thisMaze.setStart(tr, tc);
                    }
                    else
                    {
                        sfxManager.playSfx("WallPlace");
                        if (thisMaze.getStart().x == tr && thisMaze.getStart().y == tc)
                        {
                            thisMaze.setStart(-1, -1);
                        }
                        thisMaze.setEnd(tr, tc);
                    }
                    reloadStartEnd();
                    changeTool(0);
                }
            }
            if (Input.GetMouseButtonUp(0) && !mouseOffReset)
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

        
        if (QualitySettings.GetQualityLevel() > 1)
        {
            lightWallOpa += Time.deltaTime * 0.1f;
            lightWallOpa %= Mathf.PI * 2;
            setLightOpaci((Mathf.Sin(lightWallOpa) + 1f) / 2f) ;
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

        float mouseTranY = mousePos.y + mazeHeight / rowMaze / scaleMaze;
        float mouseTranX = mousePos.x;

        if (mouseTranX >= ULMaze.x && mouseTranX <= DRMaze.x &&
            mouseTranY <= ULMaze.y && mouseTranY >= DRMaze.y)
        {

            float tileX = (mouseTranX - ULMaze.x) * columnMaze / mazeWidth;
            float tileY = rowMaze - (mouseTranY - DRMaze.y) * rowMaze / mazeHeight;

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


            
            if (((int)tileY == 0 && dir == 'U') || ((int)tileY == rowMaze - 1 && dir == 'D')
                || ((int)tileX == 0 && dir == 'L') || ((int)tileX == columnMaze - 1 && dir == 'R'))
            {
                return (-1, -1, '?');
            }

                return ((int)tileY, (int)tileX, dir);
        }

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

                wallLMaze[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Corner" + tran.Item1 + "_Light");
                wallLMaze[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * tran.Item2);

            }
        }
        reloadDiffMaze();
    }

    public void subMaze()
    {
        //Calculate maze here
        if (thisMaze.getDifficultyMaze() >= 0f)
        {
            sfxManager.playSfx("Click");
            mazeButtonsPanel.SetActive(false);
            mainBGM.fadeVolume(0.6f, 1.2f);


            foreach (GameObject e in overButton)
            {
                e.SetActive(true);
            }
            animations.SetTrigger("Submit");
            phase = 2;
        }
    }

    public void resetMaze()
    {
        sfxManager.playSfx("Click");
        thisMaze.reMaze();
        reloadWalls();
        reloadStartEnd();
        changeTool(0);
    }

    public void changeTool(int id)
    {
        /* 0 is wall
         * 1 is Erase
         * 2 is setStart
         * 3 is setEnd
        */

        ColorBlock notSe = new ColorBlock
        {
            normalColor = NormalButtonColor,
            highlightedColor = HoverButtonColor,
            pressedColor = ActivateColor,
            selectedColor = ActivateColor,
            colorMultiplier = 2f,
            fadeDuration = 0.1f
            
        };

        ColorBlock Se = new ColorBlock
        {
            normalColor = ActivateColor,
            highlightedColor = ActivateColor,
            pressedColor = ActivateColor,
            selectedColor = ActivateColor,
            colorMultiplier = 2f,
            fadeDuration = 0.1f
        };

        for (int i = 0; i < toolsButtons.Length; i++)
        {
            if(toolsButtons[i] != null)
                toolsButtons[i].colors = notSe;
        }
        toolsButtons[id].colors = Se;

        tool = (short)id;

    }

    void reloadStartEnd()
    {
        staObject.SetActive(thisMaze.getStart().x != -1 && thisMaze.getStart().y != -1);
        flgObject.SetActive(thisMaze.getEnd().x != -1 && thisMaze.getEnd().y != -1);

        staObject.transform.localPosition = posMazeToV3(thisMaze.getStart().x, thisMaze.getStart().y);
        staObject.transform.localScale = new Vector3(scaleMaze, scaleMaze, 1f);
        flgObject.transform.localPosition = posMazeToV3(thisMaze.getEnd().x, thisMaze.getEnd().y);
        flgObject.transform.localScale = new Vector3(scaleMaze, scaleMaze, 1f);
        reloadDiffMaze();
    }

    void reloadDiffMaze()
    {
        TextMesh thisTextDiff = DiffText.GetComponent<TextMesh>();
        float diff = thisMaze.getDifficultyMaze();
        bool isSolve = false;
        if (diff == -1)
        {
            thisTextDiff.text = LangManager.calling("CDNoStart");
        }
        else if(diff == -2)
        {
            thisTextDiff.text = LangManager.calling("CDNoEnd");
        }
        else if(diff == -3)
        {
            thisTextDiff.text = LangManager.calling("CDUnsol");
        }
        else
        {
            isSolve = true;
            thisTextDiff.text = string.Format(LangManager.calling("CDDiff"), diff);
        }

        subButton.interactable = isSolve;

    }

    public void setLightOpaci(float opa)
    {
        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                wallLMaze[i, j].GetComponent<SpriteRenderer>().color = new Color(mainColor[curTurn - 1].r,
                    mainColor[curTurn - 1].g, mainColor[curTurn - 1].b, opa);
            }
        }
    }

}
