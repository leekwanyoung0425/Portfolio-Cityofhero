using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public Transform trContents;
    public TMP_InputField myTextInput = null;
    public Scrollbar verticalScroll = null;
    bool ignoreNextReturn = false;
    public GameObject chatItem;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Return))
        {
            if(ignoreNextReturn)
            {
                ignoreNextReturn = false;
            }
            else
            {
                myTextInput.ActivateInputField();
            }
        }
    }

    public void AddChat(string str)
    {
        if(str.Length >0)
        {
            GameObject obj = Instantiate(chatItem);
            obj.transform.SetParent(trContents);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            ChatItem scp = obj.GetComponent<ChatItem>();
            scp.SetText(str);
            myTextInput.text = "";
            myTextInput.ActivateInputField();
            StartCoroutine(SetScrollZeroValue(10.0f));
        }
        else
        {
            ignoreNextReturn = true;
            myTextInput.ActivateInputField();
        }
    }

    IEnumerator SetScrollZeroValue(float speed)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        while (verticalScroll.value>Mathf.Epsilon)
        {
            float delta = Time.smoothDeltaTime * speed;
            verticalScroll.value = Mathf.Clamp(verticalScroll.value - delta, 0f, 1f);
            yield return null;
        }
    }

    public void TextLengthCheck()
    {
        if (myTextInput.text.Length > 5)
        {
            int index = myTextInput.text.Length;

            while (myTextInput.text.Length > 5)
            {
                myTextInput.text.Remove(index-1);
                --index;
            }
        }
    }
}
