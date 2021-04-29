﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject skillData = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {

        skillData = eventData.selectedObject;
        if (skillData == null) return;
           
        GameObject childObj = null;         
        GameObject parentObj;

        if (this.gameObject.transform.childCount > 0)
        {
            childObj = this.gameObject.transform.GetChild(0).gameObject;
        }


            parentObj = skillData.GetComponent<SkillDrag>().curParentObj;

            //if (skillData.GetComponent<SkillDrag>().isDrag)
            //{
                if (skillData.GetComponent<SkillDataBase>() != null)
                {
                    if (parentObj.GetComponent<Slot>() == null && childObj == null)
                    {
                    Debug.Log("들1");
                        bool skillOverlapChk = false;
                        GameObject alreadySkillinslot = null;
                        foreach (GameObject skill in SlotData.GetInstance().slots)
                        {
                            if (skill.transform.childCount > 0)
                            {
                                if (skill.GetComponentInChildren<SkillDataBase>().skillName == skillData.GetComponent<SkillDataBase>().skillName)
                                {
                                    alreadySkillinslot = skill.gameObject.transform.GetChild(0).gameObject;
                                    skillOverlapChk = true;
                                    break;
                                }
                            }
                        }

                        if (skillOverlapChk)
                        {
                            skillData.transform.SetParent(this.gameObject.transform);
                            skillData.transform.localPosition = Vector3.zero;
                            Destroy(alreadySkillinslot);
                        }
                        else
                        {
                            skillData.transform.SetParent(this.gameObject.transform);
                            skillData.transform.localPosition = Vector3.zero;
                        }

                    }
                    else if (parentObj.GetComponent<Slot>() == null && childObj != null)
                    {
                    Debug.Log("들2");
                    Destroy(childObj);
                        skillData.transform.SetParent(this.gameObject.transform);
                        skillData.transform.localPosition = Vector3.zero;
                    }
                    else if (parentObj.GetComponent<Slot>() != null && childObj == null)
                    {
                    Debug.Log("들3");
                    skillData.transform.SetParent(this.gameObject.transform);
                        skillData.transform.localPosition = Vector3.zero;
                    }
                    else if (parentObj.GetComponent<Slot>() != null && childObj != null)
                    {
                    Debug.Log("들4");
                    childObj.transform.SetParent(parentObj.transform);
                        childObj.transform.localPosition = Vector3.zero;
                        skillData.transform.SetParent(this.gameObject.transform);
                        skillData.transform.localPosition = Vector3.zero;
                    }

                }
            //}
        
    }

}
