using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPointClick : MonoBehaviour
{
    public GameObject cardinalPoint;
    public GameObject pointImg;
    public Transform myTr;
    bool set = false;
    // Start is called before the first frame update
    public void PointClick()
    {
        set = !set;
        pointImg.SetActive(set);
        cardinalPoint.GetComponent<CardinalPoints>().isSelect = set;
        cardinalPoint.GetComponent<CardinalPoints>().SelectPoint(myTr);

    }
}
