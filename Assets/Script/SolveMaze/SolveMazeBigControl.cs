using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolveMazeBigControl : MonoBehaviour
{

    public EachPlayer p1Field;
    public EachPlayer p2Field;
    Animator animator;

    float tim = 0f;
    public float[] beatSample = new float[1];
    float nowOpa = 0f;

    bool waitForPlayer = true;
    int curTurn = 1;

    public TextMeshProUGUI BigText;
    public Text NextText;
    public Text WallText;
    

    bool duringTransit = false;

    public float timeBegin = 30f;
    float timeRem = 0f;
    public TextMeshProUGUI timeText;

    bool isCheat = false;
    bool endGame = false;
    public GameObject WhatMeme;
    public AudioClip WhatMusic;
    bool backToMenu = true;
    public GameObject cheatAppear;
    public GameObject Char;
    public Text wonText;
    public ParticleSystem par1;
    public ParticleSystem par2;

    PauseManager thisPause;

    public BGMManager Bgm;

    // Start is called before the first frame update
    void Start()
    {

        Bgm.fadeVolume(0.4f);
        Bgm.startPlaying();

        animator = GetComponent<Animator>();

        GameDataManager.loadGame();
        GameDataManager.setPhase("Solving");
        GameDataManager.saveGame();

        timeRem = 30f;

        thisPause = GetComponent<PauseManager>();

        if (GameDataManager.player1Maze.getDifficultyMaze() < GameDataManager.player2Maze.getDifficultyMaze())
        {
            curTurn = 2;
        }
        else
        {
            curTurn = 1;
        }



        ReloadText();
        rePosition();
        if (GameDataManager.player1Maze.getDifficultyMaze() < 0f || GameDataManager.player2Maze.getDifficultyMaze() < 0f)
        {
            isCheat = true;
            endGame = true;
        }

        wonText.font = LangManager.titleTextFont;
        timeText.font = TMP_FontAsset.CreateFontAsset(LangManager.textFont);
        BigText.font = TMP_FontAsset.CreateFontAsset(LangManager.titleTextFont);
        NextText.font = LangManager.titleTextFont;
        WallText.font = LangManager.titleTextFont;
    }

    void BoomCheat()
    {
        WhatMeme.SetActive(true);
        cheatAppear.SetActive(true);
        Bgm.ChangeSongAndPlay(WhatMusic, WhatMusic);
    }


    // Update is called once per frame
    void Update()
    {
        
        
        tim += Time.deltaTime;
        tim %=  2 * Mathf.PI;

        float newOpa = Mathf.Clamp01(Mathf.Log(Mathf.Max(beatSample[0] / 0.1f,0f)) + 1);
        nowOpa = Mathf.Lerp(nowOpa, newOpa, 0.2f);

        p1Field.setLightOpaci(nowOpa);
        p2Field.setLightOpaci(nowOpa);

        

        if (Input.GetKeyDown(KeyCode.Return) && endGame && !backToMenu)
        {

            GameDataManager.ResetIt();
            Bgm.fadeVolume(0f, 2f);

            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.MainMenu);
            backToMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !endGame && !duringTransit)
        {
            thisPause.callPause();
        }

        if (Input.GetKeyDown(KeyCode.Return) && waitForPlayer && !thisPause.isPause)
        {
            waitForPlayer = false;
            Bgm.fadeVolume(1f);

            if (isCheat)
            {
                BoomCheat();
            }
            else
            {
                p1Field.isPlayable = (curTurn == 1);
                p2Field.isPlayable = (curTurn == 2);
            }
            backToMenu = false;
            animator.SetTrigger("PressEnter");
        }

        

        if (!waitForPlayer && !duringTransit && !endGame && !thisPause.isPause)
        {
            if (timeRem > 0f)
            {
                timeRem -= Time.deltaTime;
                timeText.text = ((int)Mathf.Ceil(timeRem)).ToString("000");
            }
            else
            {
                timeText.text = "000";
                timeOut();
                
            }
        }

    }

    private void ReloadText()
    {
        BigText.text = string.Format(LangManager.calling("SPSolve"), curTurn);
        NextText.color = (curTurn == 1) ? p1Field.mainColor : p2Field.mainColor;
        NextText.text = string.Format(LangManager.calling("SPTurn"), curTurn);
    }

    public void wallHited()
    {
        if (!duringTransit)
        {
            duringTransit = true;
            p1Field.isPlayable = false;
            p2Field.isPlayable = false;
            curTurn = (curTurn == 1) ? 2 : 1;
            WallText.text = LangManager.calling("SPWallHit");
            animator.SetTrigger("WallHit");
            StartCoroutine(transitToAnother());
        }
    }

    public void timeOut()
    {
        if (!duringTransit)
        {
            duringTransit = true;
            p1Field.isPlayable = false;
            p2Field.isPlayable = false;
            curTurn = (curTurn == 1) ? 2 : 1;
            WallText.text = LangManager.calling("SPTimeOut");
            animator.SetTrigger("WallHit");
            StartCoroutine(transitToAnother());
        }
    }


    public void rePosition()
    {
        float scale = Mathf.Min(7f / GameDataManager.getColumnMaze(),1f);

        //5/4      16/9
        //-5.5 and -6.76
        //2.42 and 3.96

        float perDel = (((float)Screen.width / Screen.height) - (5f / 4f)) / (16f/9f-5f/4f);


        //5/4
        Vector3 activePos = new Vector3(-5.5f - 1.26f * perDel, 1.94f);
        Vector3 nonActivePos = new Vector3(2.42f + 1.54f * perDel, 0.87f);


        p1Field.mazePos = (curTurn == 1) ? activePos : nonActivePos;
        p2Field.mazePos = (curTurn == 2) ? activePos : nonActivePos;
        p1Field.transform.localScale = (curTurn == 1) ? new Vector3(scale, scale, 0.5f) : new Vector3(scale * 0.5f, scale * 0.5f, 0.5f);
        p2Field.transform.localScale = (curTurn == 2) ? new Vector3(scale, scale, 0.5f) : new Vector3(scale * 0.5f, scale * 0.5f, 0.5f);
    }

    IEnumerator transitToAnother()
    {
        Bgm.fadeVolume(0.5f,0.8f);
        yield return new WaitForSeconds(3);
        ReloadText();
        rePosition();
        timeRem = timeBegin;
        timeText.text = ((int)Mathf.Ceil(timeRem)).ToString("000");
        waitForPlayer = true;
        duringTransit = false;
        Bgm.fadeVolume(1f, 0.8f);
    }

    public void GameWon(int player,Color pColor)
    {
        if (!isCheat)
        {
            endGame = true;
            duringTransit = true;
            p1Field.isPlayable = false;
            p2Field.isPlayable = false;
            wonText.text = string.Format(LangManager.calling("SPWon"), player);
            wonText.color = pColor;
            par1.Play();
            par2.Play();

            animator.SetTrigger("EndTrig");
        }
        
    }
}
