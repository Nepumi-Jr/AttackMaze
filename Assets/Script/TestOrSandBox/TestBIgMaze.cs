using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBIgMaze : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameDataManager.player1Maze = new MazeManager();
        GameDataManager.player2Maze = new MazeManager();
        Debug.Log(GameDataManager.player1Maze);
        GameDataManager.saveGame();
        GameDataManager.loadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
