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
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int count = 0;
            Transform headTr;
            Transform leftHandTr;
            Transform rightHandTr;
            Transform leftFootTr;
            Transform rightFootTr;


            if (Physics.Raycast(ray, out hit, 1000.0f, Monster))
            {

                InsfabtargetImage.Clear();
                foreach (Image image in prefabtargetImage)
                {                    
                    InsfabtargetImage.Add(Instantiate(image));
                    InsfabtargetImage[count].transform.SetParent(FindObjectOfType<Canvas>().transform);
                    ++count;                                      
                }

                headTr = hit.transform.GetComponentInChildren<GetHeadPosition>().tr;
                leftHandTr = hit.transform.GetComponentInChildren<GetLeftHandPosition>().tr;
                rightHandTr = hit.transform.GetComponentInChildren<GetRightHandPosition>().tr;
                leftFootTr = hit.transform.GetComponentInChildren<GetLeftFootPosition>().tr;
                rightFootTr = hit.transform.GetComponentInChildren<GetRightFootPosition>().tr;

                if (isTargetingFollow != null) StopCoroutine(isTargetingFollow);
                isTargetingFollow = StartCoroutine(TargetingFollow(hit.transform, InsfabtargetImage , headTr, leftHandTr, rightHandTr, leftFootTr, rightFootTr));
              
            }
        }
    }

    IEnumerator TargetingFollow(Transform target, List<Image> images, Transform headTr, Transform leftHandTr, Transform rightHandTr, Transform leftFootTr, Transform rightFootTr)
    {
        List<float> screenPosX = new List<float>();
        List<float> screenPosY = new List<float>();

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



        while (target != null)
        {
            screenPosX.Clear();
            screenPosY.Clear();

            headPos = headTr.position;
            leftHandPos = leftHandTr.position;
            rightHandPos = rightHandTr.position;
            leftFootPos = leftFootTr.position;
            rightFootPos = rightFootTr.position;

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


            Debug.Log("최소값x" + getMinX);
            Debug.Log("최소값y" + getMinY);
            Debug.Log("최대값x" + getMaxX);
            Debug.Log("최대값y" + getMaxY);

            Debug.Log("이미지 좌측 상단 위치" + images[0].transform.localPosition);
            Debug.Log("이미지 우측 상단 위치" + images[1].transform.localPosition);
            Debug.Log("이미지 우측 하단 위치" + images[2].transform.localPosition);
            Debug.Log("이미지 좌측 하단 위치" + images[3].transform.localPosition);

            images[0].transform.localPosition = new Vector3(getMinX, getMaxY, 0);
            images[1].transform.localPosition = new Vector3(getMaxX, getMaxY, 0);
            images[2].transform.localPosition = new Vector3(getMaxX, getMinY, 0);
            images[3].transform.localPosition = new Vector3(getMinX, getMinY, 0);

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
