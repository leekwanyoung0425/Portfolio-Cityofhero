using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public GameObject mapUI;
    public Transform npcPoint;
    public Transform characterPoint;
    public Transform platerTr;
    public Transform npcTr;
    Vector2 charPos = Vector2.zero;
    Vector2 npcPos = Vector2.zero;
    Vector3 charRot = Vector3.zero;
    bool mapOnOff = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        charPos.x = (platerTr.position.x * 400f) / 350f;
        charPos.y = (platerTr.position.z * 400f) / 350f;

        charRot.z = platerTr.rotation.eulerAngles.y;
        characterPoint.localPosition = charPos;

        characterPoint.rotation = Quaternion.Euler(0, 0, charRot.z);

        npcPos.x = (npcTr.position.x * 400f) / 350f;
        npcPos.y = (npcTr.position.z * 400f) / 350f;
        npcPoint.localPosition = npcPos;
    }

    public void MapOnOff()
    {
        if (mapOnOff)
        {
            mapOnOff = false;
            mapUI.SetActive(false);
        }
        else
        {
            mapOnOff = true;
            mapUI.SetActive(true);
        }
    }
}
