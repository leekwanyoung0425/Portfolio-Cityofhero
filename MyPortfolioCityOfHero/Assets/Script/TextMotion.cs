using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMotion : MonoBehaviour
{
    private TMP_Text myText;
    private RectTransform myTextScale;
    private Vector3 size;
    private Coroutine coroutineChk;

    void Start()
    {
        size.x = 0.3f;
        size.y = 0.3f;
        size.z = 1.0f;
        myText = GetComponent<TextMeshProUGUI>();
        myTextScale = GetComponent<RectTransform>();
        myTextScale.localScale = size;
    }

    // Update is called once per frame
    void Update()
    {
        if(myTextScale.localScale.x <= 1.0f)
        myTextScale.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0);

        if (myTextScale.localScale.x >= 1.0f)
        {
            if(coroutineChk ==null)
                coroutineChk = StartCoroutine(TextHide(myText));
        }

    }

    IEnumerator TextHide(TMP_Text text)
    {
        Color col = myText.color;
        while (myText.color.a >= Mathf.Epsilon)
        {
            col.a -= 0.5f * Time.deltaTime;
            myText.color = col;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
