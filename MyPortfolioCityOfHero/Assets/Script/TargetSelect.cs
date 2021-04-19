using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetSelect : MonoBehaviour
{
    public LayerMask Monster;
    public List<Image> targetImage = new List<Image>();
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    Coroutine isTargetingFollow = null;
 
    public Transform GetselectTarget { get; private set; }
    public bool GetIsSelect { get; private set; }

    public GameObject targetUIGroup;
    Slider hpbar;
    public TMP_Text nameText;

    Transform prevSelectTarget = null;

 
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
            Transform headTr;
            Transform leftHandTr;
            Transform rightHandTr;
            Transform leftFootTr;
            Transform rightFootTr;
            

            if (Physics.Raycast(ray, out hit, 100.0f, Monster))
            {   
                if(prevSelectTarget != null && prevSelectTarget != hit.transform)
                {
                    prevSelectTarget.GetComponentInChildren<GetMaterial>().GetmyMaterial[1].SetFloat("Boolean_AB71AB7D", 0.0f);
                    Destroy(prevSelectTarget.GetComponentInChildren<MonsterState>().hpbarObj);
                    prevSelectTarget = hit.transform;
                }
                else
                {
                    prevSelectTarget = hit.transform;
                }

               GetIsSelect = true;
               GetselectTarget = hit.transform;

               headTr = hit.transform.GetComponentInChildren<GetHeadPosition>().tr;
               leftHandTr = hit.transform.GetComponentInChildren<GetLeftHandPosition>().tr;
               rightHandTr = hit.transform.GetComponentInChildren<GetRightHandPosition>().tr;
               leftFootTr = hit.transform.GetComponentInChildren<GetLeftFootPosition>().tr;
               rightFootTr = hit.transform.GetComponentInChildren<GetRightFootPosition>().tr;

               if (isTargetingFollow != null) StopCoroutine(isTargetingFollow);
               isTargetingFollow = StartCoroutine(TargetingFollow(hit.transform, targetImage, headTr, leftHandTr, rightHandTr, leftFootTr, rightFootTr));               
            }
        }
    }

    IEnumerator TargetingFollow(Transform target, List<Image> images, Transform headTr, Transform leftHandTr, Transform rightHandTr, Transform leftFootTr, Transform rightFootTr)
    {

        Vector3 setPos = Vector3.zero;
        target.GetComponentInChildren<GetMaterial>().GetmyMaterial[1].SetFloat("Boolean_AB71AB7D", 1.0f);
        target.GetComponentInChildren<MonsterState>().InstantiateHPbar();
        hpbar = target.GetComponentInChildren<MonsterState>().myhpbar;
        targetUIGroup.SetActive(true);
        MonsterData targetData = target.GetComponent<MonsterData>();
        hpbar.maxValue = targetData.GetMaxHp;
        hpbar.value = targetData.GetCurHp;
        nameText.text = targetData.GetName;
        nameText.outlineColor = new Color(255,0,0);

        float targetingMaxDistance =  70.0f;
        float targetDistance = 0.0f;

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


        while (target != null && GetIsSelect)
        {
            hpbar.value = targetData.GetCurHp;
            float dist = (transform.position - target.position).magnitude;

            if (dist >= outRange)
            {
                GetIsSelect = false;
                target.GetComponentInChildren<GetMaterial>().GetmyMaterial[1].SetFloat("Boolean_AB71AB7D", 0.0f);
                Destroy(hpbar.gameObject);
                targetUIGroup.SetActive(false);
                target = null;
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

                targetDistance = Mathf.Abs(screenPosX[4] - screenPosX[0]);

                if (targetDistance < targetingMaxDistance)
                {
                    images[0].transform.localPosition = new Vector3(getMinX - 20.0f, getMaxY, 0);
                    images[1].transform.localPosition = new Vector3(getMaxX + 20.0f, getMaxY, 0);
                    images[2].transform.localPosition = new Vector3(getMaxX + 20.0f, getMinY, 0);
                    images[3].transform.localPosition = new Vector3(getMinX - 20.0f, getMinY, 0);
 
                }
                else
                {
                    images[0].transform.localPosition = new Vector3(getMinX, getMaxY, 0);
                    images[1].transform.localPosition = new Vector3(getMaxX, getMaxY, 0);
                    images[2].transform.localPosition = new Vector3(getMaxX, getMinY, 0);
                    images[3].transform.localPosition = new Vector3(getMinX, getMinY, 0);
                }

                setPos.x = Mathf.Lerp(getMinX, getMaxX, 0.5f);
                setPos.y = getMaxY+10.0f;
                hpbar.transform.localPosition = setPos;
                setPos.y += 30.0f;
                nameText.transform.localPosition = setPos;
            }
            yield return null;
        }
        targetUIGroup.SetActive(false);
    }
}
