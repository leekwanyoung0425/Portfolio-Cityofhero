using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardinalPoints : MonoBehaviour
{
    public Transform playerTr;
    public Image cardinalPoints;
    public Image selectPoint;
    public Transform monster;
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
        myMaterial.SetTextureOffset("_MainTex", new Vector2(setoffSet,0));
        SelectPoint();
    }

    public void SelectPoint()
    {
        monsteralgle = monster.eulerAngles.y;
        monstersetoffSet = (algle / 360.0f) + 0.94f;
        selectPoint.rectTransform.localPosition = new Vector2(monstersetoffSet*800,0);
    }
}
