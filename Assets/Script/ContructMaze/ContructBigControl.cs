using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContructBigControl : MonoBehaviour
{


    public Text textDisDes;
    int curTurn = 1;
    bool duringTrans = true;
    public Animator animations;
    public TextMeshProUGUI bigMazeText;

    public List<GameObject> overButton;

    int phase = 0;
    /* 0 is wait
     * 1 is Contruct
     * 2 is sure ?
    */


    void Start()
    {

        if (GameDataManager.getPhase() == "ConP1")
        {
            curTurn = 1;
        }
        else
        {
            curTurn = 2;
        }

        textDisDes.text = string.Format(LangManager.calling("CDisDes"), curTurn, curTurn == 1 ? 2 : 1);
        bigMazeText.text = string.Format(LangManager.calling("CMaze"), curTurn);
        StartCoroutine(waitTran(1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(!duringTrans)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (phase == 0)
                {
                    phase = 1;
                    animations.SetTrigger("WarnEnter");
                    StartCoroutine(inActiveThem());
                }
                if (phase == 2)
                {
                    phase = 3;
                    animations.SetTrigger("Proceed");

                    StartCoroutine(nextBruh());
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && phase == 2)
            {
                animations.SetTrigger("BackEdit");
                StartCoroutine(waitTran(1f));
                Debug.Log("A");
                phase = 1;
                StartCoroutine(inActiveThem());
                Debug.Log("B");
            }
        }
    }

    IEnumerator waitTran(float halt)
    {
        duringTrans = true;
        yield return new WaitForSeconds(halt);
        duringTrans = false;
    }

    IEnumerator inActiveThem()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject e in overButton)
        {
            e.SetActive(false);
        }
    }

    IEnumerator nextBruh()
    {
        duringTrans = true;
        yield return new WaitForSeconds(3f);

        if (curTurn == 1)
        {
            GameDataManager.setPhase("ConP2");
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.ContructMaze);
        }
        else
        {
            GameDataManager.setPhase("Solving");
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.SolveMaze);
        }

        
    }

    public void subMaze()
    {
        //Calculate maze here
        if (true)
        {
            foreach (GameObject e in overButton)
            {
                e.SetActive(true);
            }
            animations.SetTrigger("Submit");
            phase = 2;
        }
    }

}
