using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardinalPoints : MonoBehaviour
{
    public Transform playerTr;
    public Image cardinalPoints;
    public Image selectPoint;
    Material myMaterial;
    float algle;
    float monsteralgle;
    float setoffSet;
    float monstersetoffSet;

    void Start()
    {
        myMaterial = cardinalPoints.materialForRendering;
    }

    // Update is called once per frame
    void Update()
    {
        algle = playerTr.eulerAngles.y;
        setoffSet = (algle / 360.0f)+0.94f;
        if(setoffSet > 1)
        {
            setoffSet -= 1.0f;
        }
        myMaterial.SetTextureOffset("_MainTex", new Vector2(setoffSet,0));
    }

    public void SelectPoint(Transform selectTarget)
    {

        Vector3 dir = (selectTarget.position- playerTr.position).normalized;
        float rot = Vector3.Dot(dir, playerTr.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;

        if (Vector3.Dot(playerTr.right, dir) < 0.0f)
        {
            rot *= -1.0f;
        }

        monstersetoffSet = (400f*rot)/180f + 0.94f;
        
        selectPoint.rectTransform.localPosition = new Vector2(monstersetoffSet, 0);
    }
}
