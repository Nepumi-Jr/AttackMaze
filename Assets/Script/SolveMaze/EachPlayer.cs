using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EachPlayer : MonoBehaviour
{

    public int player = 1;
    public bool isPlayable = true;
    public Color mainColor;
    int rowMaze;
    int columnMaze;

    float fieldWidth;
    float fieldHeight;

    public float CX;
    public float CY;

    GameObject[,] field;
    GameObject[,] border;
    GameObject[,] borderLight;
    GameObject[,] fieldPassed;
    GameObject chara;
    GameObject finishFlag;
    Vector2Int nowCharaPos;
    float rotX = 0f;
    float rotZ = 0f;

    public Vector3 mazePos;
    Vector3 vibPos;
    float vib = 0f;
    float vibTime = 0f;

    MazeManager bigPMaze;
    MazeManager seenWalls;

    public SolveMazeBigControl bigControl;

    short[,] fieldCanWalked;

    public Material charMaterial;
    bool boomEnd = false;




    // Start is called before the first frame update
    void Start()
    {
        Sprite picBgField = Resources.Load<Sprite>("Texture/MazeField/BgField");
        fieldWidth = picBgField.rect.width / picBgField.pixelsPerUnit;
        fieldHeight = picBgField.rect.height / picBgField.pixelsPerUnit;

        

        rowMaze = GameDataManager.getRowMaze();
        columnMaze = GameDataManager.getColumnMaze();
        
        if (player == 1)
        {
            bigPMaze = GameDataManager.player1Maze;
        }
        else
        {
            bigPMaze = GameDataManager.player2Maze;
        }

        seenWalls = new MazeManager(rowMaze, columnMaze);
        field = new GameObject[rowMaze, columnMaze];
        fieldPassed = new GameObject[rowMaze, columnMaze];
        border = new GameObject[rowMaze + 1, columnMaze + 1];
        borderLight = new GameObject[rowMaze + 1, columnMaze + 1];
        fieldCanWalked = new short[rowMaze, columnMaze];

        CX = - fieldWidth * columnMaze / 2 + fieldWidth / 2;
        CY = fieldHeight * rowMaze / 2 - fieldHeight / 2;

        finishFlag = new GameObject(string.Format("FL{0}", player));
        finishFlag.AddComponent<SpriteRenderer>();
        finishFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Finish");
        finishFlag.GetComponent<SpriteRenderer>().sortingLayerName = "MazeFlag";
        finishFlag.transform.parent = this.transform;
        finishFlag.transform.localPosition = new Vector3(
                    fieldWidth * (bigPMaze.getEnd().y), -fieldHeight * (bigPMaze.getEnd().x), 0f);
        finishFlag.transform.localScale = new Vector3(1f,1f,1f);


        float wallZoom = picBgField.rect.width;
        wallZoom /= Resources.Load<Sprite>("Texture/MazeField/CornerZ_Line").rect.width;
        for (int i = 0; i < rowMaze; i++)
        {
            for(int j = 0; j < columnMaze; j++)
            {
                field[i, j] = new GameObject(
                    string.Format("GP{0}[{1},{2}]", player, i, j));
                field[i, j].AddComponent<SpriteRenderer>();
                field[i, j].GetComponent<SpriteRenderer>().sprite = picBgField;
                field[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBG";
                field[i, j].transform.parent = this.transform;
                field[i, j].transform.localPosition = new Vector3(
                    fieldWidth * j, -fieldHeight * i, 0f);
                field[i, j].transform.localScale = new Vector3(
                    1f,1f,1f);

                fieldPassed[i, j] = new GameObject(string.Format("GPw{0}[{1},{2}]", player, i, j));
                fieldPassed[i, j].AddComponent<SpriteRenderer>();
                fieldPassed[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/PassZ");
                fieldPassed[i, j].GetComponent<SpriteRenderer>().color = mainColor;
                fieldPassed[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBorder";
                fieldPassed[i, j].transform.parent = this.transform;
                fieldPassed[i, j].transform.localPosition = new Vector3(
                    fieldWidth * j, -fieldHeight * i, 0f);
                fieldPassed[i, j].transform.localScale = new Vector3(
                    1f, 1f, 1f);
                fieldCanWalked[i, j] = 0;
            }
        }

        chara = Instantiate(bigControl.Char);
        chara.SetActive(true);
        chara.transform.parent = this.transform;
        chara.transform.localScale = new Vector3(25f, 25f, 25f);
        nowCharaPos = bigPMaze.getStart();
        chara.transform.localPosition = new Vector3(0f, 0f);
        chara.GetComponent<MeshRenderer>().material = charMaterial;

        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                border[i, j] = new GameObject(
                    string.Format("WP{0}[{1},{2}]", player, i, j));
                border[i, j].AddComponent<SpriteRenderer>();
                border[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerE_Line");
                border[i, j].GetComponent<SpriteRenderer>().color = mainColor;
                border[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBorder";
                border[i, j].transform.parent = this.transform;
                border[i, j].transform.localPosition = new Vector3(
                    fieldWidth  * (j - 0.5f), -fieldHeight * (i - 0.5f), -1f);
                border[i, j].transform.localScale = new Vector3(wallZoom, wallZoom);

                borderLight[i, j] = new GameObject(
                    string.Format("WP{0}[{1},{2}]", player, i, j));
                borderLight[i, j].AddComponent<SpriteRenderer>();
                borderLight[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerZ_Light");
                borderLight[i, j].GetComponent<SpriteRenderer>().sortingLayerName = "MazeBorder";
                borderLight[i, j].transform.parent = this.transform;
                borderLight[i, j].transform.localPosition = new Vector3(
                    fieldWidth * (j - 0.5f), -fieldHeight * (i - 0.5f), -1f);
                borderLight[i, j].transform.localScale = new Vector3(wallZoom, wallZoom);
            }
        }
        reloadWalls();


    }

    private void wallHit()
    {
        vib = 0.2f;
        reloadWalls();
        bigControl.wallHited();
    }

    public void setLightOpaci(float per)
    {
        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                borderLight[i, j].GetComponent<SpriteRenderer>().color = new Color(1f, 0.7f, 0.7f, per);
            }
        }
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
                    if (seenWalls.isPass(i - 1, j - 1, 'R') == false)
                    {
                        state |= 1;
                    }
                    if (seenWalls.isPass(i - 1, j - 1, 'D') == false)
                    {
                        state |= 8;
                    }
                }
                if (i < rowMaze && j < columnMaze)
                {
                    if (seenWalls.isPass(i, j, 'U') == false)
                    {
                        state |= 2;
                    }
                    if (seenWalls.isPass(i, j, 'L') == false)
                    {
                        state |= 4;
                    }
                }
                if (i - 1 >= 0 && j < columnMaze)
                {
                    if (seenWalls.isPass(i - 1, j, 'D') == false)
                    {
                        state |= 2;
                    }
                    if (seenWalls.isPass(i - 1, j, 'L') == false)
                    {
                        state |= 1;
                    }
                }
                if (i < rowMaze && j - 1 >= 0)
                {
                    if (seenWalls.isPass(i, j - 1, 'R') == false)
                    {
                        state |= 4;
                    }
                    if (seenWalls.isPass(i, j - 1, 'U') == false)
                    {
                        state |= 8;
                    }
                }

                (string, short) tran = wallShortToIt(state);
                border[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Corner" + tran.Item1 + "_Line");
                border[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * tran.Item2);

                borderLight[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Corner" + tran.Item1 + "_Light");
                borderLight[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * tran.Item2);

            }
        }
    }

    void fieldPassReload(int i,int j)
    {
        (string, short) res = wallShortToIt(fieldCanWalked[i, j]);
        fieldPassed[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Pass" + res.Item1);
        fieldPassed[i, j].transform.localRotation = Quaternion.Euler(0f, 0f, 90f * res.Item2);
    }

    // Update is called once per frame
    void Update()
    {

        if (vib > 0f)
        {
            vibTime += Time.deltaTime;
            vibTime %= 2 * Mathf.PI;
            vibPos = new Vector3(Mathf.Cos(vibTime*79f) * vib, Mathf.Sin(vibTime * 67f) * vib);
            vib -= Time.deltaTime / 2f;
        }

        rotZ = (rotZ + Time.deltaTime * 70f) % 360f;


        this.transform.localPosition = mazePos + vibPos;

        if (isPlayable)
        { 
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rotX = 180f;
                if (bigPMaze.isPass(nowCharaPos.x, nowCharaPos.y, 'L'))
                {
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y] |= 8;
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y - 1] |= 2;
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y);
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y - 1);

                    nowCharaPos += new Vector2Int(0, -1);
                }
                else
                {
                    if (seenWalls.isPass(nowCharaPos.x, nowCharaPos.y, 'L'))
                    {
                        seenWalls.toggleWall(nowCharaPos.x, nowCharaPos.y, 'L');
                    }
                    wallHit();
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rotX = 0f;

                if (bigPMaze.isPass(nowCharaPos.x, nowCharaPos.y, 'R'))
                {
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y] |= 2;
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y + 1] |= 8;
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y);
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y + 1);
                    nowCharaPos += new Vector2Int(0, 1);
                }
                else
                {
                    if (seenWalls.isPass(nowCharaPos.x, nowCharaPos.y, 'R'))
                    {
                        seenWalls.toggleWall(nowCharaPos.x, nowCharaPos.y, 'R');
                    }
                    wallHit();
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rotX = 270f;
                if (bigPMaze.isPass(nowCharaPos.x, nowCharaPos.y, 'U'))
                {
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y] |= 1;
                    fieldCanWalked[nowCharaPos.x - 1, nowCharaPos.y ] |= 4;
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y);
                    fieldPassReload(nowCharaPos.x - 1, nowCharaPos.y);
                    nowCharaPos += new Vector2Int(-1, 0);
                }
                else
                {
                    if (seenWalls.isPass(nowCharaPos.x, nowCharaPos.y, 'U'))
                    {
                        seenWalls.toggleWall(nowCharaPos.x, nowCharaPos.y, 'U');
                    }
                    wallHit();
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                rotX = 90f;

                if (bigPMaze.isPass(nowCharaPos.x, nowCharaPos.y, 'D'))
                {
                    fieldCanWalked[nowCharaPos.x, nowCharaPos.y] |= 4;
                    fieldCanWalked[nowCharaPos.x + 1, nowCharaPos.y] |= 1;
                    fieldPassReload(nowCharaPos.x, nowCharaPos.y);
                    fieldPassReload(nowCharaPos.x + 1, nowCharaPos.y);
                    nowCharaPos += new Vector2Int(1, 0);
                }
                else
                {
                    if (seenWalls.isPass(nowCharaPos.x, nowCharaPos.y, 'D'))
                    {
                        seenWalls.toggleWall(nowCharaPos.x, nowCharaPos.y, 'D');
                    }
                    wallHit();
                }
            }
            
        }

        float speed = 12f;

        //chara.transform.localPosition = new Vector3(nowCharaPos.y * fieldWidth, -nowCharaPos.x * fieldHeight);
        //chara.transform.localRotation = Quaternion.Euler(rotX, 90f, rotZ);

        chara.transform.localPosition = Vector3.Lerp(chara.transform.localPosition, new Vector3(nowCharaPos.y * fieldWidth, -nowCharaPos.x * fieldHeight), Time.deltaTime * speed);
        chara.transform.localRotation = Quaternion.Lerp(chara.transform.localRotation, Quaternion.Euler(rotX, 90f, rotZ), Time.deltaTime * speed);

        if (nowCharaPos.x == bigPMaze.getEnd().x && nowCharaPos.y == bigPMaze.getEnd().y && !boomEnd)
        {
            boomEnd = true;
            bigControl.GameWon(player, mainColor);
        }


    }
}
