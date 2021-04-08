using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{

    float nowTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= 60f)
        {
            GameDataManager.saveGame();
            Debug.Log("Saved<" + Random.Range(0,9));
            nowTime -= 60f;
        }
    }


}
