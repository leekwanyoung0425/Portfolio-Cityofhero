using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatItem : MonoBehaviour
{
    public TMP_Text myText = null;
    public TMP_Text nicknameText = null;
    // Start is called before the first frame update
    public void SetText(string str, string name)
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        Vector2 textsize = myText.GetPreferredValues(str);

        int line = (int)(textsize.x / myText.GetComponent<RectTransform>().sizeDelta.x) + 1;
        
        size.y = (float)line * textsize.y;
        this.GetComponent<RectTransform>().sizeDelta = size;

        myText.text = str;
        nicknameText.text = name + ":";
    }
}
