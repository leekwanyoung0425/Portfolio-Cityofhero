using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPointClick : MonoBehaviour
{
    public GameObject cardinalPoint;
    public GameObject pointImg;
    bool set = false;
    // Start is called before the first frame update
    public void PointClick()
    {
        cardinalPoint.GetComponent<CardinalPoints>().SelectPoint(this.transform);
        set = !set;
        pointImg.SetActive(set);
    }
}
