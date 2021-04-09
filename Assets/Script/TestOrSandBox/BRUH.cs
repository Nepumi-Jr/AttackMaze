using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRUH : MonoBehaviour
{

    MazeManager thisMaze;

    // Start is called before the first frame update

    int[,] DP;
    HashSet<Vector2Int> pathing = new HashSet<Vector2Int>(); 

    int DFS(Vector2Int pos)
    {
        Debug.Log(pos.x + "-" + pos.y);
        for (int i = 0; i < thisMaze.getGridRow(); i++)
        {
            string str = "";
            for (int j = 0; j < thisMaze.getGridColumn(); j++)
            {
                str += DP[i, j] + " ";
            }
            Debug.Log(str);
        }

        if (pos.x == thisMaze.getEnd().x && pos.y == thisMaze.getEnd().y)
        {
            return 1;
        }

        if(DP[pos.x, pos.y] != -1)
        {
            return DP[pos.x, pos.y];
        }

        pathing.Add(pos);

        int paths = 0;

        if (thisMaze.isPosInMaze(pos + new Vector2Int(-1, 0)) &&
            thisMaze.isPass(pos.x, pos.y, 'U') &&
            !pathing.Contains(pos + new Vector2Int(-1, 0)))
        {
            paths += DFS(pos + new Vector2Int(-1, 0));
        }

        if (thisMaze.isPosInMaze(pos + new Vector2Int(0, 1)) &&
            thisMaze.isPass(pos.x, pos.y, 'R') &&
            !pathing.Contains(pos + new Vector2Int(0, -1)))
        {
            paths += DFS(pos + new Vector2Int(0, 1));
        }

        if (thisMaze.isPosInMaze(pos + new Vector2Int(1, 0)) &&
            thisMaze.isPass(pos.x, pos.y, 'D') &&
            !pathing.Contains(pos + new Vector2Int(1, 0)))
        {
            paths += DFS(pos + new Vector2Int(1, 0));
        }

        if (thisMaze.isPosInMaze(pos + new Vector2Int(0, -1)) &&
            thisMaze.isPass(pos.x, pos.y, 'L') &&
            !pathing.Contains(pos + new Vector2Int(0, -1)))
        {
            paths += DFS(pos + new Vector2Int(0, -1));
        }

        DP[pos.x, pos.y] = paths;

        pathing.Remove(pos);
        return paths;
    }

    void Start()
    {
        GameDataManager.loadGame();
        thisMaze = GameDataManager.player1Maze;
        Debug.Log(thisMaze);
        DP = new int[thisMaze.getGridRow(), thisMaze.getGridColumn()];
        for (int i = 0; i < thisMaze.getGridRow(); i++)
        {
            for(int j = 0;j < thisMaze.getGridColumn(); j++)
            {
                DP[i, j] = -1;
            }
        }

        Debug.Log("There are " + DFS(thisMaze.getStart()) + " Paths");
        for (int i = 0; i < thisMaze.getGridRow(); i++)
        {
            string str = "";
            for (int j = 0; j < thisMaze.getGridColumn(); j++)
            {
                str += DP[i, j] + " ";
            }
            Debug.Log(str);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
