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
    public GameObject selectIamgePanel;
    Vector2 halfsize = Vector2.zero;
    Coroutine isTargetingFollow = null;
 
    public Transform GetselectTarget { get; private set; }
    public bool GetIsSelect { get; private set; }

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
            if (InsfabtargetImage != null)
            {
                foreach (Image image in InsfabtargetImage)
                {
                    Destroy(image.gameObject);
                }
                InsfabtargetImage.Clear();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int count = 0;
            Transform headTr;
            Transform leftHandTr;
            Transform rightHandTr;
            Transform leftFootTr;
            Transform rightFootTr;


            if (Physics.Raycast(ray, out hit, 100.0f, Monster))
            {   
               GetIsSelect = true;
               GetselectTarget = hit.transform;

               foreach (Image image in prefabtargetImage)
               {
                   InsfabtargetImage.Add(Instantiate(image));
                   InsfabtargetImage[count].transform.SetParent(selectIamgePanel.transform);
                   ++count;
               }

               headTr = hit.transform.GetComponentInChildren<GetHeadPosition>().tr;
               leftHandTr = hit.transform.GetComponentInChildren<GetLeftHandPosition>().tr;
               rightHandTr = hit.transform.GetComponentInChildren<GetRightHandPosition>().tr;
               leftFootTr = hit.transform.GetComponentInChildren<GetLeftFootPosition>().tr;
               rightFootTr = hit.transform.GetComponentInChildren<GetRightFootPosition>().tr;

               if (isTargetingFollow != null) StopCoroutine(isTargetingFollow);
               isTargetingFollow = StartCoroutine(TargetingFollow(hit.transform, InsfabtargetImage, headTr, leftHandTr, rightHandTr, leftFootTr, rightFootTr));               
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

        float outRange = 15.0f;


        while (target != null || GetIsSelect)
        {
            float dist = (transform.position - target.position).magnitude;

            if (dist >= outRange)
            {
                GetIsSelect = false;
                foreach (Image image in InsfabtargetImage)
                {
                    Destroy(image.gameObject);
                }
                InsfabtargetImage.Clear();
                StopCoroutine(isTargetingFollow);
            }
            else
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

                screenPosX.Sort();
                screenPosY.Sort();

                getMinX = screenPosX[0];
                getMinY = screenPosY[0];
                getMaxX = screenPosX[4];
                getMaxY = screenPosY[4];

                images[0].transform.localPosition = new Vector3(getMinX, getMaxY, 0);
                images[1].transform.localPosition = new Vector3(getMaxX, getMaxY, 0);
                images[2].transform.localPosition = new Vector3(getMaxX, getMinY, 0);
                images[3].transform.localPosition = new Vector3(getMinX, getMinY, 0);
            }
            yield return null;
        }
    }
}
