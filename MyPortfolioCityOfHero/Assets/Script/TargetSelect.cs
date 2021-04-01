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
        List<Vector3> screenPos = new List<Vector3>();
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


        headPos = target.GetComponentInChildren<GetHeadPosition>().pos;
        leftHandPos = target.GetComponentInChildren<GetLeftHandPosition>().pos;
        rightHandPos = target.GetComponentInChildren<GetRightHandPosition>().pos;
        leftFootPos = target.GetComponentInChildren<GetLeftFootPosition>().pos;
        rightFootPos = target.GetComponentInChildren<GetRightFootPosition>().pos;


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
            leftHandScreenPos.y -= halfsize.x;
            rightHandScreenPos.x -= halfsize.x;
            rightHandScreenPos.y -= halfsize.x;
            leftFootScreenPos.x -= halfsize.x;
            leftFootScreenPos.y -= halfsize.x;
            rightFootScreenPos.x -= halfsize.x;
            rightFootScreenPos.y -= halfsize.x;


            screenPos.Add(headScreenPos);
            screenPos.Add(leftHandScreenPos);
            screenPos.Add(rightHandScreenPos);
            screenPos.Add(leftFootScreenPos);
            screenPos.Add(rightFootScreenPos);



            images[0].transform.localPosition = test;
            images[0].transform.localPosition += new Vector3(-50.0f, 0, 0);
            images[1].transform.localPosition = test;
            images[1].transform.localPosition += new Vector3(50.0f, 0, 0);


            //images[2].transform.localPosition = targetUIPosition;
            //images[2].transform.localPosition += new Vector3(70.0f, -50.0f, 0);
            //images[3].transform.localPosition = targetUIPosition;
            //images[3].transform.localPosition += new Vector3(-70.0f, -50.0f, 0);

            yield return null;
        }
    }

    List<Vector3> GetMinMaxScreenPosition(List<Vector3> pos)
    {
        List<Vector3> minMaxPos = new List<Vector3>();
        float minX = 0.0f;
        float maxX = 0.0f;
        float minY = 0.0f;
        float maxY = 0.0f;


        for (int i=0; i< pos.Count; i++)
        {
            if(pos[i].x <= pos[i+1].x) minX = pos[i].x;
            if(pos[i].y <= pos[i+1].y) minY = pos[i].x;
        }



        return 
    }
}
