using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetIndicator : MonoBehaviour
{
    public GameObject target;
    public Camera mainCamera;
    public Canvas canvas;
    public Transform playerTr;
    public Transform npcTr;
    

    public Image targetIndicatorImage;
    public TMP_Text targetIndicatordist;
    public Image OffScreenTargetIndicatorImage;
    public TMP_Text OffScreenTargetIndicatordist;
    public float OutOfSightOffset = 20f;
    private float outOffSightOffset { get {return OutOfSightOffset; } }

    private RectTransform canvasRect;

    private RectTransform targetRectTransform;
    private RectTransform offTargetRectTransform;
    private RectTransform textRectTransform;



    // Start is called before the first frame update
    void Start()
    {
        targetRectTransform = targetIndicatorImage.gameObject.GetComponent<RectTransform>();
        offTargetRectTransform = OffScreenTargetIndicatorImage.gameObject.GetComponent<RectTransform>();
        textRectTransform = OffScreenTargetIndicatordist.gameObject.GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }


    public void Update()
    {
        SetIndicatorPosition();
    }

    public void SetIndicatorPosition()
    {
        Vector3 indicatorPosition = mainCamera.WorldToScreenPoint(target.transform.position);

        //캐릭터 화면 안에 적의 좌표가 들어오고 실제로도 보일때
        if(indicatorPosition.z >= 0f & indicatorPosition.x <= canvasRect.rect.width * canvasRect.localScale.x
            & indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x & indicatorPosition.x >= 0f & indicatorPosition.y >= 0f)
        {
            indicatorPosition.z = 0f;
            TargetOutOfSight(false, indicatorPosition);
            targetRectTransform.position = indicatorPosition;
            targetIndicatordist.text = (Vector3.Distance(playerTr.position, npcTr.position)).ToString("N0") + "m";
        }
        //적의 좌표가 앞쪽이지만 화면에 안들어올때
        else if (indicatorPosition.z >= 0f)
        {
            indicatorPosition = OutofRangeIndicatorPositionB(indicatorPosition);
            TargetOutOfSight(true, indicatorPosition);
            offTargetRectTransform.position = indicatorPosition;
            OffScreenTargetIndicatordist.text = (Vector3.Distance(playerTr.position, npcTr.position)).ToString("N0") + "m";
            if (offTargetRectTransform.position.y >= 1065.0f)
            {
                textRectTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y - 30.0f, indicatorPosition.z);
            }
            else
            {
                textRectTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y+ 30.0f, indicatorPosition.z);
            }
        }
        //적 좌표가 뒤쪽일때 
        else
        {
            indicatorPosition *= -1f;
            indicatorPosition = OutofRangeIndicatorPositionB(indicatorPosition);
            TargetOutOfSight(true, indicatorPosition);
            offTargetRectTransform.position = indicatorPosition;
            OffScreenTargetIndicatordist.text = (Vector3.Distance(playerTr.position, npcTr.position)).ToString("N0") + "m";
            if (offTargetRectTransform.position.y >= 1065.0f)
            {
                textRectTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y - 100.0f, indicatorPosition.z);
            }
            else
            {
                textRectTransform.position = new Vector3(indicatorPosition.x, indicatorPosition.y+30.0f, indicatorPosition.z);
            }
        }
    }

    private Vector3 OutofRangeIndicatorPositionB(Vector3 indicatorPosition)
    {
        indicatorPosition.z = 0f;
        float canvasCenterX = canvasRect.rect.width * 0.5f;
        float canvasCenterY = canvasRect.rect.height * 0.5f;
        Vector3 canvasCenter = new Vector3(canvasCenterX, canvasCenterY, 0f) * canvasRect.localScale.x;
        indicatorPosition -= canvasCenter;

        float divX = (canvasCenterX - outOffSightOffset) / Mathf.Abs(indicatorPosition.x);
        float divY = (canvasCenterY - outOffSightOffset) / Mathf.Abs(indicatorPosition.y);

        if(divX < divY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasCenterX-outOffSightOffset)* canvasRect.localScale.x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad*angle) * indicatorPosition.x;
        }
        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);
            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasCenterY - outOffSightOffset) * canvasRect.localScale.y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }
        indicatorPosition += canvasCenter;

        return indicatorPosition;
    }

    private void TargetOutOfSight(bool outOfSight, Vector3 indicatorPosition)
    {
        if(outOfSight)
        {
            if (OffScreenTargetIndicatorImage.gameObject.activeSelf == false)
            {
                OffScreenTargetIndicatorImage.gameObject.SetActive(true);
                OffScreenTargetIndicatordist.gameObject.SetActive(true);
            }
            if (targetIndicatorImage.isActiveAndEnabled == true)
            {
                targetIndicatorImage.enabled = false;
                targetIndicatordist.enabled = false;
            }

            OffScreenTargetIndicatorImage.rectTransform.rotation = Quaternion.Euler(RotationOutOfSightTargetIndicator(indicatorPosition));
        }
        else
        {
            if (OffScreenTargetIndicatorImage.gameObject.activeSelf == true)
            {
                OffScreenTargetIndicatorImage.gameObject.SetActive(false);
                OffScreenTargetIndicatordist.gameObject.SetActive(false);
            }
            if (targetIndicatorImage.isActiveAndEnabled == false)
            {
                targetIndicatorImage.enabled = true;
                targetIndicatordist.enabled = true;
            }
        }
    }

    private Vector3 RotationOutOfSightTargetIndicator(Vector3 indicatorPosition)
    {
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width*0.5f, canvasRect.rect.height * 0.5f, 0f) * canvasRect.localScale.x;
        float angle = Vector3.SignedAngle(-Vector3.up, indicatorPosition - canvasCenter, Vector3.forward);

        return new Vector3(0f, 0f, angle);
    }

}
