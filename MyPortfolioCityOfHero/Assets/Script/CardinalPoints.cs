using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardinalPoints : MonoBehaviour
{
    public Transform playerTr;
    public Image cardinalPoints;
    Material myMaterial;
    float algle;
    float setoffSet;
    void Start()
    {
        myMaterial = cardinalPoints.materialForRendering;
    }

    // Update is called once per frame
    void Update()
    {
        algle = playerTr.eulerAngles.y;
        setoffSet = algle / 360.0f;
        myMaterial.SetTextureOffset("_MainTex", new Vector2(setoffSet,0));
    }
}
