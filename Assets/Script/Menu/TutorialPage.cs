using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{

    public Button pre;
    public Button nex;
    public GameObject[] pagesObject;
    public string[] pageId;

    int curPage = 0;
    int maxPage;

    

    void Start()
    {
        maxPage = Mathf.Min(pageId.Length, pagesObject.Length);
    }

    public void setPage(int pageNum)
    {
        curPage = Mathf.Clamp(pageNum,0,maxPage-1);
        for(int i = 0; i < maxPage; i++)
        {
            pagesObject[i].SetActive(curPage == i);
        }

        pagesObject[curPage].transform.GetChild(0).GetComponent<Text>().text = LangManager.calling("MTM" + pageId[curPage]);
        pagesObject[curPage].transform.GetChild(0).GetComponent<Text>().font = LangManager.titleTextFont;
        pagesObject[curPage].transform.GetChild(1).GetComponent<Text>().text = LangManager.calling("MTD" + pageId[curPage]);
        pagesObject[curPage].transform.GetChild(1).GetComponent<Text>().font = LangManager.textFont;

        pre.interactable = curPage != 0;
        nex.interactable = curPage != (maxPage - 1);
    }

    public void nextPage()
    {
        curPage++;
        setPage(curPage);
    }

    public void previousPage()
    {
        curPage--;
        setPage(curPage);
    }


}
