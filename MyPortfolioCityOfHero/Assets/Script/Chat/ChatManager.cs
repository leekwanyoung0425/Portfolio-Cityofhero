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
    public bool ignoreNextReturn = false;
    public GameObject chatItem;
    public GameObject alert;
    public TMP_Text alertText;

    private static ChatManager instance;
    public static ChatManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<ChatManager>();
        }
        return instance;
    }

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
                AddChat(myTextInput.text);
            }
            else
            {
                ignoreNextReturn = true;
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

        ignoreNextReturn = false;
        myTextInput.DeactivateInputField();
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
        if (myTextInput.text.Length > 10)
        {
            alertText.text = "10자 이내로 입력해주세요";
            alert.SetActive(true);
            myTextInput.text = myTextInput.text.Substring(0, 10);                        
        }
    }
}
