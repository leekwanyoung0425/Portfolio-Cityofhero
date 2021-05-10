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
    public GameObject chatBubble;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    public Transform uipos;
    public Transform uiParentpos;
    GameObject chatbubbleObj =null;

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
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myTextInput.isFocused) ignoreNextReturn = true;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(ignoreNextReturn)
            {
                AddChat();
            }
            else
            {
                ignoreNextReturn = true;
                myTextInput.ActivateInputField();               
            }
        }
    }

    public void AddChat()
    {
        if(myTextInput.text.Length >0)
        {
            GameObject obj = Instantiate(chatItem);
            obj.transform.SetParent(trContents);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            ChatItem scp = obj.GetComponent<ChatItem>();
            scp.SetText(myTextInput.text , PlayerData.GetInstance().playerName);

            if (chatbubbleObj != null) Destroy(chatbubbleObj);
            chatbubbleObj = Instantiate(chatBubble);
            chatbubbleObj.transform.SetParent(uiParentpos);
            Vector3 textpos = Camera.main.WorldToScreenPoint(uipos.position);
            textpos.x -= halfsize.x;
            textpos.y -= halfsize.y;
            textpos.y += 30.0f;
            chatbubbleObj.transform.localPosition = textpos;
            chatbubbleObj.GetComponent<ChatBubble>().chatBuuble(myTextInput.text);
            Destroy(chatbubbleObj, 5.0f);

            myTextInput.text = "";
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
        if (myTextInput.text.Length > 20)
        {
            alertText.text = "20자 이내로 입력해주세요";
            alert.SetActive(true);
            myTextInput.text = myTextInput.text.Substring(0, 20);                        
        }
    }
}
