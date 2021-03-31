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
        Debug.Log(halfsize);
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
        Transform tr = target.Find("Head"); //HeadTop_End
        Debug.Log(tr);
        Vector3 headPos = tr.position;

        Vector3 min_headPos;
        Vector3 max_headPos;
        Vector3 min_rightHandPos;
        Vector3 max_rightHandPos;
        Vector3 min_leftHandPos;
        Vector3 max_leftHandPos;
        Vector3 max_footPos;

        while (target != null)
        {
            max_headPos = Camera.main.WorldToScreenPoint(headPos);
            max_headPos.x -= halfsize.x;
            max_headPos.y -= halfsize.y;

            images[0].transform.localPosition = max_headPos;
            images[0].transform.localPosition += new Vector3(-50.0f, 0, 0);
            images[1].transform.localPosition = max_headPos;
            images[1].transform.localPosition += new Vector3(50.0f, 0, 0);
            //images[2].transform.localPosition = targetUIPosition;
            //images[2].transform.localPosition += new Vector3(70.0f, -50.0f, 0);
            //images[3].transform.localPosition = targetUIPosition;
            //images[3].transform.localPosition += new Vector3(-70.0f, -50.0f, 0);

            yield return null;
        }
    }


}
