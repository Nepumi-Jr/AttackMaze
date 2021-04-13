using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager
{
    private int gridRow = 5;
    private int gridColumn = 5;
    private short[,] mazeField;
    private float diffMaze = -3f;
    private bool iscaled = false;

    private Vector2Int posStart;
    private Vector2Int posEnd;

    public bool[,] playable;

    private void initMaze()
    {
        posStart = new Vector2Int(-1, -1);
        posEnd = new Vector2Int(-1, -1);
        mazeField = new short[gridRow,gridColumn];
        for (int i = 0; i < gridRow; i++)
        {
            for (int j = 0; j < gridColumn; j++)
            {
                mazeField[i, j] = 15;
            }
        }

        for (int i = 1; i < gridColumn - 1; i++)
        {
            mazeField[0 , i] = 14;
        }
        for (int i = 1; i < gridColumn - 1; i++)
        {
            mazeField[gridRow - 1, i] = 11;
        }

        for (int i = 1; i < gridRow - 1; i++)
        {
            mazeField[i, gridColumn - 1] = 13;
        }
        for (int i = 1; i < gridRow - 1; i++)
        {
            mazeField[i, 0] = 7;
        }

        mazeField[0, 0] = 6;
        mazeField[0, gridColumn - 1] = 12;
        mazeField[gridRow - 1, 0] = 3;
        mazeField[gridRow - 1, gridColumn - 1] = 9;

    }

    public MazeManager()
    {
        gridRow = 5;
        gridColumn = 5;
        initMaze();
    }

    public MazeManager(int gridRow, int gridColumn)
    {
        this.gridRow = gridRow;
        this.gridColumn = gridColumn;
        initMaze();
    }

    public void reMaze()
    {
        iscaled = false;
        initMaze();
    }

    public void forceSetMaze(short[,] maze)
    {
        iscaled = false;
        for (int i = 0;i < gridRow;i++)
        {
            for (int j = 0;j < gridColumn;j++)
            {
                mazeField[i, j] = maze[i, j];
            }
        }
    }

    public bool isPosInMaze(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridRow)
        {
            return false;
        }
        if (pos.y < 0 || pos.y >= gridColumn)
        {
            return false;
        }
        return true;
    }

    public bool isPass(int row, int column, char dir = 'U')
    {
        if (!isPosInMaze(new Vector2Int(row, column)))
        {
            Debug.LogError("Maze : unknown position");
            return false;
        }

        switch (dir)
        {
            case 'U':
            case 'u':

                return (mazeField[row, column] & 1) > 0;

            case 'D':
            case 'd':

                return (mazeField[row, column] & 4) > 0;

            case 'L':
            case 'l':

                return (mazeField[row, column] & 8) > 0;

            case 'R':
            case 'r':

                return (mazeField[row, column] & 2) > 0;

            default:
                Debug.LogError("Maze : Moving Unknown Direction");
                return false;
        }
    }

    public void toggleWall(int row, int column, char dir = 'U')
    {
        iscaled = false;
        if (!isPosInMaze(new Vector2Int(row, column)))
        {
            Debug.LogError("Maze : Calling toggle oversize");
            return;
        }

        switch(dir)
        {
            case 'U':
            case 'u':

                if (row != 0)
                {
                    mazeField[row - 1, column] ^= 4;
                }
                mazeField[row, column] ^= 1;

                break;
            case 'D':
            case 'd':

                if (row != gridRow - 1)
                {
                    mazeField[row + 1, column] ^= 1;
                }
                mazeField[row, column] ^= 4;

                break;
            case 'L':
            case 'l':

                if (column != 0)
                {
                    mazeField[row, column - 1] ^= 2;
                }
                mazeField[row, column] ^= 8;

                break;
            case 'R':
            case 'r':

                if (column != gridColumn - 1)
                {
                    mazeField[row, column + 1] ^= 8;
                }
                mazeField[row, column] ^= 2;

                break;
            default:
                Debug.LogError("Maze : Calling toggle Unknown Direction");
                break;
        }
    }

    public void setStart(int row, int column)
    {
        if (!isPosInMaze(new Vector2Int(row, column)))
        {
            Debug.LogWarning("Maze : set start oversize");
        }
        posStart = new Vector2Int(row, column);
        iscaled = false;
    }

    public void setEnd(int row, int column)
    {
        if (!isPosInMaze(new Vector2Int(row, column)))
        {
            Debug.LogWarning("Maze : set start oversize");
        }
        posEnd = new Vector2Int(row, column);
        iscaled = false;
    }

    public Vector2Int getStart()
    {
        return posStart;
    }

    public Vector2Int getEnd()
    {
        return posEnd;
    }

    private void difficultyMaze()
    {
        iscaled = true;
        if (!isPosInMaze(posStart))
        {
            diffMaze = -1f;
            return;
        }
        if (!isPosInMaze(posEnd))
        {
            diffMaze = -2f;
            return;
        }


        int walls = 0;
        for(int i = 0; i < gridRow - 1; i++)
        {
            for(int j = 0; j < gridColumn - 1; j++) {

                if (!isPass(i, j, 'R')) walls++;
                if (!isPass(i, j, 'D')) walls++;
            }
        }


        bool[,] flg = new bool[gridRow, gridColumn];
        //BFS Area goes here
        Queue<(int, int)> quea = new Queue<(int, int)>();
        playable = new bool[gridRow, gridColumn];
        playable[posStart.x, posStart.y] = true;
        quea.Enqueue((posStart.x, posStart.y));
        while(quea.Count > 0)
        {
            (int, int) now = quea.Dequeue();
            int nowRow = now.Item1;
            int nowColumn = now.Item2;
            //Left side
            if (nowColumn > 0 && ((mazeField[nowRow, nowColumn] & 8) != 0) && playable[nowRow, nowColumn - 1] == false)
            {
                playable[nowRow, nowColumn - 1] = true;
                quea.Enqueue((nowRow, nowColumn - 1));
            }

            //right side
            if (nowColumn + 1 < gridColumn && ((mazeField[nowRow, nowColumn] & 2) != 0) && playable[nowRow, nowColumn + 1] == false)
            {
                playable[nowRow, nowColumn + 1] = true;
                quea.Enqueue((nowRow, nowColumn + 1));
            }

            //Up side
            if (nowRow > 0 && ((mazeField[nowRow, nowColumn] & 1) != 0) && playable[nowRow - 1, nowColumn] == false)
            {
                playable[nowRow - 1, nowColumn] = true;
                quea.Enqueue((nowRow - 1, nowColumn));
            }

            //right side
            if (nowRow + 1 < gridRow && ((mazeField[nowRow, nowColumn] & 4) != 0) && playable[nowRow + 1, nowColumn] == false)
            {
                playable[nowRow + 1, nowColumn] = true;
                quea.Enqueue((nowRow + 1, nowColumn));
            }
        }


        //BFS maze goes here
        Queue<(int, int, int, int, char)> que = new Queue<(int, int, int, int, char)>();

        que.Enqueue((posStart.x, posStart.y, 0, 0, '?'));
        flg[posStart.x, posStart.y] = true;
        while (que.Count > 0)
        {
            (int, int, int, int, char) now = que.Dequeue();
            int nowRow = now.Item1;
            int nowColumn = now.Item2;
            int nowDistance = now.Item3;
            int nowTurns = now.Item4;
            char nowDir = now.Item5;

            if (new Vector2Int(nowRow, nowColumn) == posEnd)
            {

                int misPath = 0;
                for(int i = 0; i < gridRow; i++)
                {
                    for(int j=0;j < gridColumn; j++)
                    {
                        if (flg[i, j]) misPath--;
                        if (playable[i, j]) misPath++;
                    }
                } 

                diffMaze = nowDistance * 0.1f + nowTurns * 1.2f + walls * 0.05f + misPath * 0.12f;
                return;
            }

            //Left side
            if (nowColumn > 0 && ((mazeField[nowRow, nowColumn] & 8) != 0) && flg[nowRow, nowColumn - 1] == false)
            {
                flg[nowRow, nowColumn - 1] = true;
                if (nowDir != 'L')
                {
                    que.Enqueue((nowRow, nowColumn - 1, nowDistance + 1, nowTurns + 1, 'L'));
                }
                else
                {
                    que.Enqueue((nowRow, nowColumn - 1, nowDistance + 1, nowTurns, 'L'));
                }
            }

            //right side
            if (nowColumn + 1 < gridColumn && ((mazeField[nowRow, nowColumn] & 2) != 0) && flg[nowRow, nowColumn + 1] == false)
            {
                flg[nowRow, nowColumn + 1] = true;
                if (nowDir != 'R')
                {
                    que.Enqueue((nowRow, nowColumn + 1, nowDistance + 1, nowTurns + 1, 'R'));
                }
                else
                {
                    que.Enqueue((nowRow, nowColumn + 1, nowDistance + 1, nowTurns, 'R'));
                }
            }

            //Up side
            if (nowRow > 0 && ((mazeField[nowRow, nowColumn] & 1) != 0) && flg[nowRow - 1, nowColumn] == false)
            {
                flg[nowRow - 1, nowColumn] = true;
                if (nowDir != 'U')
                {
                    que.Enqueue((nowRow - 1, nowColumn, nowDistance + 1, nowTurns + 1, 'U'));
                }
                else
                {
                    que.Enqueue((nowRow - 1, nowColumn, nowDistance + 1, nowTurns, 'U'));
                }
            }

            //right side
            if (nowRow + 1 < gridRow && ((mazeField[nowRow, nowColumn] & 4) != 0) && flg[nowRow + 1, nowColumn] == false)
            {
                flg[nowRow + 1, nowColumn] = true;
                if (nowDir != 'D')
                {
                    que.Enqueue((nowRow + 1, nowColumn, nowDistance + 1, nowTurns + 1, 'D'));
                }
                else
                {
                    que.Enqueue((nowRow + 1, nowColumn, nowDistance + 1, nowTurns, 'D'));
                }
            }


        }

        diffMaze = -3f;
        return;
    }

    public float getDifficultyMaze()
    {
        if (!iscaled)
        {
            difficultyMaze();
        }
        return diffMaze;
    }

    private string numToPic(int num)
    {
        switch(num)
        {
            case 0: return " ";
            case 1: return "v";
            case 2: return "<";
            case 3: return "╚";
            case 4: return "^";
            case 5: return "║";
            case 6: return "╔";
            case 7: return "╠";
            case 8: return ">";
            case 9: return "╝";
            case 10: return "═";
            case 11: return "╩";
            case 12: return "╗";
            case 13: return "╣";
            case 14: return "╦";
            default: return "╬";
        }
    }

    public override string ToString() {
        string res = "";
        for(int i = 0; i < gridRow; i++)
        {
            for(int j = 0; j < gridColumn; j++)
            {
                res += numToPic(mazeField[i, j]) + "";
            }
            res += "\n";
        }
        return res.TrimEnd();
    }

    private string shortToHex(short num)
    {
        switch (num)
        {
            case 0: return "0";
            case 1: return "1";
            case 2: return "2";
            case 3: return "3";
            case 4: return "4";
            case 5: return "5";
            case 6: return "6";
            case 7: return "7";
            case 8: return "8";
            case 9: return "9";
            case 10: return "A";
            case 11: return "B";
            case 12: return "C";
            case 13: return "D";
            case 14: return "E";
            default: return "F";
        }
    }

    private short hexToShort(char num)
    {
        switch (num)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            case 'A': return 10;
            case 'B': return 11;
            case 'C': return 12;
            case 'D': return 13;
            case 'E': return 14;
            case 'F': return 15;
            default: return -1;
        }
    }

    public string dumpMaze()
    {
        string dumping = "";
        dumping += gridRow.ToString() + ';';
        dumping += gridColumn.ToString() + ';';
        dumping += posStart.x.ToString() + ';' + posStart.y.ToString() + ';';
        dumping += posEnd.x.ToString() + ';' + posEnd.y.ToString() + ';';

        for (int i = 0; i < gridRow; i++)
        {
            for(int j = 0; j < gridColumn; j++)
            {
                dumping += shortToHex(mazeField[i, j]);
            }
        }

        return dumping;
    }

    public void loadMaze(string content)
    {
        string[] data = content.Trim().Split(';');

        try
        {
            gridRow = System.Int16.Parse(data[0]);
            gridColumn = System.Int16.Parse(data[1]);
            setStart(System.Int16.Parse(data[2]), System.Int16.Parse(data[3]));
            setEnd(System.Int16.Parse(data[4]), System.Int16.Parse(data[5]));
        }
        catch
        {
            Debug.LogError("Error during convert data back to maze");
            gridRow = 5;
            gridColumn = 5;
            initMaze();
            return;
        }

        mazeField = new short[gridRow, gridColumn];

        for (int i = 0; i < gridRow; i++)
        {
            for (int j = 0; j < gridColumn; j++)
            {
                short d = hexToShort(data[6][i * gridColumn + j]);
                if (d != -1)
                {
                    mazeField[i, j] = d;
                }
                else
                {
                    Debug.LogError("Error during convert data back to maze");
                    initMaze();
                    return;
                }
            }
        }
    }

    public int getGridRow()
    {
        return gridRow;
    }

    public int getGridColumn()
    {
        return gridColumn;
    }

}