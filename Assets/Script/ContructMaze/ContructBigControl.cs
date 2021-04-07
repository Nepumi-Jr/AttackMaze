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
    int rowMaze;
    int columnMaze;
    float fieldWidth;
    float fieldHeight;

    public Text Debuggg;
    public PauseManager pauseBruh;

    const float heightPercent = 0.1f;//Put your magic number here :)
    const float hitBoxPercent = 0.2f;
    float scaleMaze = 1f;

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

        if (GameDataManager.getPhase() == "ConP1")
        {
            curTurn = 1;
        }
        else
        {
            curTurn = 2;
        }

        textDisDes.text = string.Format(LangManager.calling("CDisDes"), curTurn, curTurn == 1 ? 2 : 1);
        bigMazeText.text = string.Format(LangManager.calling("CMaze"), curTurn);

        rowMaze = GameDataManager.getRowMaze();
        columnMaze = GameDataManager.getColumnMaze();

        tileMaze = new GameObject[rowMaze, columnMaze];

        bigMaze.localPosition = new Vector3(fieldWidth / 2 - fieldWidth * columnMaze / 2,
            fieldHeight / 2 - fieldHeight * rowMaze / 2);

        Debug.Log(">>>>>" + picBgField.rect.width);

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
                Debug.Log("A");
                phase = 1;
                StartCoroutine(inActiveThem());
                Debug.Log("B");
            }
           
            if (Input.GetMouseButton(0))
            {
                fromPos(Input.mousePosition);
                
                Debug.Log(Input.mousePosition);
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
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.ContructMaze);
        }
        else
        {
            GameDataManager.setPhase("Solving");
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
            return ((int)tileY, (int)tileX, dir);
        }
        Debuggg.text = "NOT In Maze";

        return (-1, -1, '?');
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
