using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelect : MonoBehaviour
{
    public LayerMask Monster;
    public List<Image> prefabtargetImage = new List<Image>();
    List<Image> InsfabtargetImage =  new List<Image>();
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    Coroutine isTargetingFollow = null;

    
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        Debug.Log(canvas.pixelRect.width);
        Debug.Log(canvas.pixelRect.height);
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
        Debug.Log(halfsize.x);
        Debug.Log(halfsize.y);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int count = 0;

            if (Physics.Raycast(ray, out hit, 1000.0f, Monster))
            {

                foreach (Image image in prefabtargetImage)
                {               
                    InsfabtargetImage.Add(Instantiate(image));
                    InsfabtargetImage[count].transform.SetParent(FindObjectOfType<Canvas>().transform);
                    ++count;                                      
                }

                if (isTargetingFollow != null) StopCoroutine(isTargetingFollow);
                isTargetingFollow = StartCoroutine(TargetingFollow(hit.transform, InsfabtargetImage));
            }
        }
    }

    IEnumerator TargetingFollow(Transform target, List<Image> images)
    {
        List<float> screenPosX = new List<float>();
        List<float> screenPosY = new List<float>();
        List<float> getMinMaxPos = new List<float>();

        Vector3 headPos;
        Vector3 leftHandPos;
        Vector3 rightHandPos;
        Vector3 leftFootPos;
        Vector3 rightFootPos;

        Vector3 headScreenPos;
        Vector3 leftHandScreenPos;
        Vector3 rightHandScreenPos;
        Vector3 leftFootScreenPos;
        Vector3 rightFootScreenPos;

        float getMinX;
        float getMinY;
        float getMaxX;
        float getMaxY;


        headPos = target.GetComponentInChildren<GetHeadPosition>().pos;
        leftHandPos = target.GetComponentInChildren<GetLeftHandPosition>().pos;
        rightHandPos = target.GetComponentInChildren<GetRightHandPosition>().pos;
        leftFootPos = target.GetComponentInChildren<GetLeftFootPosition>().pos;
        rightFootPos = target.GetComponentInChildren<GetRightFootPosition>().pos;

        Debug.Log(headPos);
        Debug.Log(leftHandPos);
        Debug.Log(rightHandPos);
        Debug.Log(leftFootPos);
        Debug.Log(rightFootPos);


        while (target != null)
        {

            headScreenPos = Camera.main.WorldToScreenPoint(headPos);
            leftHandScreenPos = Camera.main.WorldToScreenPoint(leftHandPos);
            rightHandScreenPos = Camera.main.WorldToScreenPoint(rightHandPos);
            leftFootScreenPos = Camera.main.WorldToScreenPoint(leftFootPos);
            rightFootScreenPos = Camera.main.WorldToScreenPoint(rightFootPos);

            headScreenPos.x -= halfsize.x;
            headScreenPos.y -= halfsize.y;
            leftHandScreenPos.x -= halfsize.x;
            leftHandScreenPos.y -= halfsize.y;
            rightHandScreenPos.x -= halfsize.x;
            rightHandScreenPos.y -= halfsize.y;
            leftFootScreenPos.x -= halfsize.x;
            leftFootScreenPos.y -= halfsize.y;
            rightFootScreenPos.x -= halfsize.x;
            rightFootScreenPos.y -= halfsize.y;


            Debug.Log(headScreenPos);
            Debug.Log(leftHandScreenPos);
            Debug.Log(rightHandScreenPos);
            Debug.Log(leftFootScreenPos);
            Debug.Log(rightFootScreenPos);

            screenPosX.Add(headScreenPos.x);
            screenPosX.Add(leftHandScreenPos.x);
            screenPosX.Add(rightHandScreenPos.x);
            screenPosX.Add(leftFootScreenPos.x);
            screenPosX.Add(rightFootScreenPos.x);

            screenPosY.Add(headScreenPos.y);
            screenPosY.Add(leftHandScreenPos.y);
            screenPosY.Add(rightHandScreenPos.y);
            screenPosY.Add(leftFootScreenPos.y);
            screenPosY.Add(rightFootScreenPos.y);


            GetMinMaxScreenPosition(ref screenPosX, ref screenPosY);

            getMinX = screenPosX[0];
            getMinY = screenPosY[0];
            getMaxX = screenPosX[4];
            getMaxY = screenPosY[4];

            Debug.Log(getMinX);
            Debug.Log(getMinY);
            Debug.Log(getMaxX);
            Debug.Log(getMaxY);

            images[0].transform.localPosition = new Vector3(getMinX, getMaxY, 0);
            images[0].transform.localPosition = new Vector3(getMaxX, getMaxY, 0);
            images[1].transform.localPosition = new Vector3(getMaxX, getMinY, 0);
            images[1].transform.localPosition = new Vector3(getMinX, getMinY, 0);

            yield return null;
        }
    }

    void GetMinMaxScreenPosition(ref List<float> posX, ref List<float> posY)
    {
    
        float tempX = 0.0f;
        float tempY = 0.0f;

        for (int i=0; i< posX.Count-1; i++)
        {
            if(posX[i] >= posX[i+1])
            {
                tempX = posX[i+1];
                posX[i + 1] = posX[i];
                posX[i] = tempX;
            }

            if (posY[i] >= posY[i + 1])
            {
                tempY = posY[i + 1];
                posY[i + 1] = posY[i];
                posY[i] = tempY;
            }
        }
    }
}
