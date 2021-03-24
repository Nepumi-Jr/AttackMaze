using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolveMazeBigControl : MonoBehaviour
{

    
    AudioSource audioSource;
    public AudioClip BGM;

    public EachPlayer p1Field;
    public EachPlayer p2Field;
    Animator animator;

    float tim = 0f;
    public float[] beatSample = new float[1];
    float nowOpa = 0f;

    bool waitForPlayer = true;
    int curTurn = 1;

    public Text BigText;
    public Text NextText;
    public Text WallText;
    

    bool duringTransit = false;

    public float timeBegin = 30f;
    float timeRem = 0f;
    public Text timeText;

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



    // Start is called before the first frame update
    void Start()
    {
        LangManager.loadLang("TH");
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = BGM;
        audioSource.Play();

        animator = GetComponent<Animator>();

        GameDataManager.loadGame();
        GameDataManager.setPhase("Solving");
        GameDataManager.saveGame();

        timeRem = 30f;
        //TODO : Compare 2 maze who make Easier maze Start first
        

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


        
    }

    void BoomCheat()
    {
        WhatMeme.SetActive(true);
        cheatAppear.SetActive(true);
        audioSource.clip = WhatMusic;
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        audioSource.GetOutputData(beatSample, 0);
        
        tim += Time.deltaTime;
        tim %=  2 * Mathf.PI;

        float newOpa = Mathf.Clamp01(Mathf.Log(Mathf.Max(beatSample[0] / 0.1f,0f)) + 1);
        nowOpa = Mathf.Lerp(nowOpa, newOpa, 0.2f);

        p1Field.setLightOpaci(nowOpa);
        p2Field.setLightOpaci(nowOpa);

        if (Input.GetKeyDown(KeyCode.Return) && endGame && !backToMenu)
        {
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.MainMenu);
            backToMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) && waitForPlayer)
        {
            waitForPlayer = false;

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

        

        if (!waitForPlayer && !duringTransit && !endGame)
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
        p1Field.mazePos = (curTurn == 1) ? new Vector3(-7.25f, 1.94f) : new Vector3(5.96f, 0.87f);
        p2Field.mazePos = (curTurn == 2) ? new Vector3(-7.25f, 1.94f) : new Vector3(5.96f, 0.87f);
        p1Field.transform.localScale = (curTurn == 1) ? new Vector3(1f, 1f, 0.5f) : new Vector3(0.5f, 0.5f, 0.5f);
        p2Field.transform.localScale = (curTurn == 2) ? new Vector3(1f, 1f, 0.5f) : new Vector3(0.5f, 0.5f, 0.5f);
    }

    IEnumerator transitToAnother()
    {
        yield return new WaitForSeconds(3);
        ReloadText();
        rePosition();
        timeRem = timeBegin;
        timeText.text = ((int)Mathf.Ceil(timeRem)).ToString("000");
        waitForPlayer = true;
        duringTransit = false;
    }

    public void GameWon(int player,Color pColor)
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
