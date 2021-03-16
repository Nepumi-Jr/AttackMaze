using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager
{
    private int gridRow = 5;
    private int gridColumn = 5;
    private short[,] mazeField;

    private Vector2Int posStart;
    private Vector2Int posEnd;

    private void initMaze()
    {
        posStart = new Vector2Int(-1, -1);
        posEnd = new Vector2Int(-1, -1);
        mazeField = new short[gridRow,gridColumn];
        for (int i = 0;i < gridRow ;i++)
        {
            for (int j = 0;j < gridColumn;j++)
            {
                mazeField[i, j] = 0;
            }
        }
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


    public void toggleWall(int row, int column, char dir = 'U')
    {
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
            Debug.LogError("Maze : set start oversize");
            return;
        }
        posStart = new Vector2Int(row, column);
    }

    public void setEnd(int row, int column)
    {
        if (!isPosInMaze(new Vector2Int(row, column)))
        {
            Debug.LogError("Maze : set start oversize");
            return;
        }
        posEnd = new Vector2Int(row, column);
    }

    public float difficultyMaze()
    {
        if (!isPosInMaze(posStart))
        {
            return -1f;
        }
        if (!isPosInMaze(posEnd))
        {
            return -2f;
        }

        bool[,] flg = new bool[gridRow, gridColumn];

        //BFS goes here
        Queue<(int,int,int,int, char)> que = new Queue<(int, int, int, int, char)>();

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

            if ( new Vector2Int(nowRow, nowColumn) == posEnd)
            {
                return nowDistance + nowTurns * 2f;
            }

            //Left side
            if (nowColumn > 0 && ((mazeField[nowRow, nowColumn] & 8) != 0) && flg[nowRow, nowColumn - 1] == false)
            {
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
            if (nowColumn <= gridColumn && ((mazeField[nowRow, nowColumn] & 2) != 0) && flg[nowRow, nowColumn + 1] == false)
            {
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
            if (nowRow <= gridRow && ((mazeField[nowRow, nowColumn] & 4) != 0) && flg[nowRow + 1, nowColumn] == false)
            {
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

        return -3f;
    }

}