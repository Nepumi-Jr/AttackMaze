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
    public static bool itemAss;
    public static bool isDebuging;

    static bool ff = false;

    static bool isLoaded = false;

    public static void init()
    {
        if (phase == "ConP1")
        {
            player1Maze = new MazeManager(5, 5);
            player2Maze = new MazeManager(5, 5);
            phase = "ConP1";
            timeStamp = new DateTimeOffset(DateTime.Now).ToString();
            itemAss = false;
        }
    }

    public GameDataManager()
    {
        init();
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
        dumping.Add("Item:" + (itemAss ? "1" : "0"));

        try
        {
            File.WriteAllLines(SAVE_PATH, dumping);
        }
        catch
        {
            Debug.LogWarning("Can't Save file...");
        }

        
    }

    public static void loadGame()
    {

        string SAVE_PATH = Application.dataPath + "/data.fgm";
        isLoaded = true;

        try
        {
            if (!File.Exists(SAVE_PATH))
            {
                init();
                return;
            }


            string[] content = File.ReadAllLines(SAVE_PATH);
            player1Maze.loadMaze(content[0].Replace("P1 Maze:", ""));
            player2Maze.loadMaze(content[1].Replace("P2 Maze:", ""));
            phase = content[2].Replace("phase:", "");
            timeStamp = content[3].Replace("Last seen:", "");
            itemAss = content[4] == "Item:1";

            if (content.Length == 6)
            {
                if (content[5] == "BugFordNaHee")
                {
                    isDebuging = true;
                }
            }
        }
        catch
        {
            init();
        }
        

        

    }

    public static void ResetIt()
    {
        string SAVE_PATH = Application.dataPath + "/data.fgm";
        try
        {
            File.Delete(SAVE_PATH);
        }
        catch
        {
            phase = "ConP1";
            init();
            Debug.LogWarning("Can't Delect data.fgm");
        }
        
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
