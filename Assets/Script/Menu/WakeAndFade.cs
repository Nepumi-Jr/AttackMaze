using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeAndFade : MonoBehaviour
{

    Image thisImage;
    float alpha = 0;

    private void Start()
    {
        thisImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.Lerp(alpha, 1f, Time.deltaTime * 4f);
        thisImage.color = new Color(0, 0, 0, alpha);
    }
}
