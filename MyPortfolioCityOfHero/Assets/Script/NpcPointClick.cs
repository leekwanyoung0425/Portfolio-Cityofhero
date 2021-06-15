using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPointClick : MonoBehaviour
{
    public GameObject cardinalPoint;
    public GameObject pointImg;
    public GameObject npcIndicator;
    public Transform myTr;
    public GameObject target;
    public Transform npcTr;
    bool set = false;
    // Start is called before the first frame update
    public void PointClick()
    {
        set = !set;
        pointImg.SetActive(set);
        npcIndicator.SetActive(set);
        npcIndicator.GetComponent<TargetIndicator>().target = this.target;
        npcIndicator.GetComponent<TargetIndicator>().npcTr = this.npcTr;
        cardinalPoint.GetComponent<CardinalPoints>().isSelect = set;
        cardinalPoint.GetComponent<CardinalPoints>().SelectPoint(myTr);

    }
}
