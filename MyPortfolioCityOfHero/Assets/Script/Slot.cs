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
        GameObject childObj = null;
        GameObject parentObj;

        if (this.gameObject.transform.childCount > 0)
        {
            childObj = this.gameObject.transform.GetChild(0).gameObject;
        }


        parentObj = skillData.GetComponent<SkillDrag>().curParentObj;

        if (skillData.GetComponent<SkillDataBase>() != null)
        {
            if(parentObj.GetComponent<Slot>() == null && childObj == null)
            {
                bool skillOverlapChk = false;
                GameObject alreadySkillinslot=null;
                foreach(SkillDataBase skill in SlotData.GetInstance().skills)
                {
                    if (skill.skillName == skillData.GetComponent<SkillDataBase>().skillName)
                    {
                        alreadySkillinslot = skill.gameObject;
                        skillOverlapChk = true;
                        break;
                    }
                }
                
                if(skillOverlapChk)
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
            else if(parentObj.GetComponent<Slot>() == null && childObj != null)
            {
                Destroy(childObj);
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
            else if(parentObj.GetComponent<Slot>() != null && childObj == null)
            {
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
            else if (parentObj.GetComponent<Slot>() != null && childObj != null)
            {
                childObj.transform.SetParent(parentObj.transform);
                childObj.transform.localPosition = Vector3.zero;
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }

            SlotData.GetInstance().SlotRenewal();
        }
        else
        {
            Destroy(eventData.selectedObject);
        }
    }

}
