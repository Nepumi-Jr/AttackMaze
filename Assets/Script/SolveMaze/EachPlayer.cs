using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EachPlayer : MonoBehaviour
{

    public int player = 1;
    public bool isPlayable = true;
    int rowMaze;
    int columnMaze;

    float fieldWidth;
    float fieldHeight;

    float CX;
    float CY;

    GameObject[,] field;
    GameObject[,] border;
    GameObject[,] borderLight;
    GameObject chara;
    Vector2Int nowCharaPos;

    Vector3 mazePos;
    Vector3 vibPos;
    float vib = 0f;
    float vibTime = 0f;

    MazeManager seenWalls;

    private char c(char x)
    {
        switch (x)
        {
            case '0': return '0';
            case '1': return '1';
            case '2': return '2';
            case '3': return '4';
            case '4': return '8';
            case '5': return '3';
            case '6': return '5';
            case '7': return '9';
            case '8': return '6';
            case '9': return 'A';
            case 'A': return 'C';
            case 'B': return 'E';
            case 'C': return 'D';
            case 'D': return 'B';
            case 'E': return '7';
            default: return 'F';
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Sprite picBgField = Resources.Load<Sprite>("Texture/MazeField/BgField");
        fieldWidth = picBgField.rect.width / picBgField.pixelsPerUnit;
        fieldHeight = picBgField.rect.height / picBgField.pixelsPerUnit;

        rowMaze = GameDataManager.getRowMaze();
        columnMaze = GameDataManager.getColumnMaze();
        seenWalls = new MazeManager(rowMaze, columnMaze);
        field = new GameObject[rowMaze, columnMaze];
        border = new GameObject[rowMaze + 1, columnMaze + 1];
        borderLight = new GameObject[rowMaze + 1, columnMaze + 1];

        CX = - fieldWidth * columnMaze / 2 + fieldWidth / 2;
        CY = fieldHeight * rowMaze / 2 - fieldHeight / 2;

        mazePos = new Vector3(CX, CY);


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
                field[i, j].transform.parent = this.transform;
                field[i, j].transform.localPosition = new Vector3(
                    fieldWidth * j, -fieldHeight * i, 0f);
            }
        }

        chara = new GameObject(string.Format("P{0}Chara", player));
        chara.transform.parent = this.transform;
        chara.AddComponent<SpriteRenderer>();
        chara.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/Capture");
        chara.transform.localScale = new Vector3(0.2f, 0.2f);
        nowCharaPos = GameDataManager.player1Maze.getStart();
        chara.transform.localPosition = new Vector3(0f, 0f);

        for (int i = 0; i < rowMaze + 1; i++)
        {
            for (int j = 0; j < columnMaze + 1; j++)
            {
                border[i, j] = new GameObject(
                    string.Format("WP{0}[{1},{2}]", player, i, j));
                border[i, j].AddComponent<SpriteRenderer>();
                border[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerE_Line");
                border[i, j].transform.parent = this.transform;
                border[i, j].transform.localPosition = new Vector3(
                    fieldWidth  * (j - 0.5f), -fieldHeight * (i - 0.5f), -1f);
                border[i, j].transform.localScale = new Vector3(wallZoom, wallZoom);

                borderLight[i, j] = new GameObject(
                    string.Format("WP{0}[{1},{2}]", player, i, j));
                borderLight[i, j].AddComponent<SpriteRenderer>();
                borderLight[i, j].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Texture/MazeField/CornerZ_Light");
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


        this.transform.localPosition = mazePos + vibPos;

        if (isPlayable)
        { 
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                chara.transform.localRotation = Quaternion.Euler(0, 0, -90f);
                if (player == 1)
                {
                    if (GameDataManager.player1Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'L'))
                    {
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
                else
                {
                    if (GameDataManager.player2Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'L'))
                    {
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
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                chara.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                if (player == 1)
                {
                    if (GameDataManager.player1Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'R'))
                    {
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
                else
                {
                    if (GameDataManager.player2Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'R'))
                    {
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
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                chara.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                if (player == 1)
                {
                    if (GameDataManager.player1Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'U'))
                    {
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
                else
                {
                    if (GameDataManager.player2Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'U'))
                    {
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
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                chara.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                if (player == 1)
                {
                    if (GameDataManager.player1Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'D'))
                    {
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
                else
                {
                    if (GameDataManager.player2Maze.isPass(nowCharaPos.x, nowCharaPos.y, 'D'))
                    {
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
            
            if (Input.anyKeyDown)
            {
                chara.transform.localPosition = new Vector3(nowCharaPos.y * fieldWidth, -nowCharaPos.x * fieldHeight);
            }
        }
    }
}
