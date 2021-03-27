using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameDataManager
{

    public static MazeManager player1Maze, player2Maze;
    protected static string timeStamp;
    protected static string phase;

    static bool isLoaded = false;

    public GameDataManager()
    {
        timeStamp = new DateTimeOffset(DateTime.Now).ToString();
        Debug.Log("Times " + timeStamp);
    }

    public static void saveGame()
    {
        timeStamp = new DateTimeOffset(DateTime.Now).ToString();
        string SAVE_PATH = Application.dataPath + "/data.fgm";
        List<string> dumping = new List<string>();
        dumping.Add("P1 Maze:" + player1Maze.dumpMaze());
        dumping.Add("P2 Maze:" + player2Maze.dumpMaze());
        dumping.Add("phase:" + phase);
        dumping.Add("Last seen:" + timeStamp);

        File.WriteAllLines(SAVE_PATH, dumping);
    }

    public static void loadGame()
    {
        string SAVE_PATH = Application.dataPath + "/data.fgm";
        player1Maze = new MazeManager();
        player2Maze = new MazeManager();
        isLoaded = true;

        string[] content = File.ReadAllLines(SAVE_PATH);
        player1Maze.loadMaze(content[0].Replace("P1 Maze:", ""));
        player2Maze.loadMaze(content[1].Replace("P2 Maze:", ""));
        phase = content[2].Replace("phase:", "");
        timeStamp = content[3].Replace("Last seen:", "");
    }

    public static void setPhase(string phasee)
    {
        if (!isLoaded) loadGame();
        phase = phasee;
    }

    public static string getPhase()
    {
        if (!isLoaded) loadGame();
        return phase;
    }

    public static int getRowMaze()
    {
        if (!isLoaded) loadGame();
        return player1Maze.getGridRow();
    }

    public static int getColumnMaze()
    {
        if (!isLoaded) loadGame();
        return player1Maze.getGridColumn();
    }

    public static string lastSeen()
    {
        if (!isLoaded) loadGame();
        return timeStamp;
    }

}
