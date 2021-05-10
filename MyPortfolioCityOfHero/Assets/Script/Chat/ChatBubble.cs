using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public TMP_Text myText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void chatBuuble(string text)
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        Vector2 textsize = myText.GetPreferredValues(text);

        int line = (int)(textsize.x / myText.GetComponent<RectTransform>().sizeDelta.x) + 1;
        if (line == 1) myText.alignment = TextAlignmentOptions.Midline;
        size.y = (float)line * textsize.y;
        this.GetComponent<RectTransform>().sizeDelta += new Vector2(0, size.y);
        myText.text = text;
    }
}
