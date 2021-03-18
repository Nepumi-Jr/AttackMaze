using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaze : MonoBehaviour
{

    MazeManager testMaze = new MazeManager(6, 6);


    private short c(char x)
    {
        switch(x)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 4;
            case '4': return 8;
            case '5': return 1+2;
            case '6': return 1+4;
            case '7': return 1+8;
            case '8': return 2+4;
            case '9': return 2+8;
            case 'A': return 4+8;
            case 'B': return 2+4+8;
            case 'C': return 1+4+8;
            case 'D': return 1+2+8;
            case 'E': return 1+2+4;
            default: return 1+2+4+8;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] tc = new string[] {
            "8A299A",
            "66899C",
            "657846",
            "629D9C",
            "E999A6",
            "594271",
        };

        short[,] ttc = new short[6, 6];


        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                ttc[i, j] = c(tc[i][j]);
            }
        }

        testMaze.forceSetMaze(ttc);
        testMaze.setStart(0, 2);
        testMaze.setEnd(2, 4);
        Debug.Log(testMaze);

        Debug.LogWarning("Maze is " + testMaze.getDifficultyMaze());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
