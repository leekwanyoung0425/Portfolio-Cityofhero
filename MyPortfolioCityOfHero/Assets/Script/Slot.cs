using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject iconData = null;
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

        iconData = eventData.selectedObject;
        if (iconData.transform.GetComponent<IconDrag>().isDrag)
        {
            GameObject childObj = null;
            GameObject parentObj;

            if (this.gameObject.transform.childCount > 0)
            {
                childObj = this.gameObject.transform.GetChild(0).gameObject;
            }
            parentObj = iconData.GetComponent<IconDrag>().curParentObj;

            if (iconData.GetComponent<SkillDataBase>() != null)
            {
                if (parentObj.GetComponent<Slot>() == null && childObj == null)
                {
                    bool skillOverlapChk = false;
                    GameObject alreadySkillinslot = null;
                    foreach (GameObject skill in SlotData.GetInstance().slots)
                    {
                        if (skill.transform.childCount > 0)
                        {
                            if (skill.GetComponentInChildren<SkillDataBase>().skillName == iconData.GetComponent<SkillDataBase>().skillName)
                            {
                                alreadySkillinslot = skill.gameObject.transform.GetChild(0).gameObject;
                                skillOverlapChk = true;
                                break;
                            }
                        }
                    }

                    if (skillOverlapChk)
                    {
                        iconData.transform.SetParent(this.gameObject.transform);
                        iconData.transform.localPosition = Vector3.zero;
                        Destroy(alreadySkillinslot);
                    }
                    else
                    {
                        iconData.transform.SetParent(this.gameObject.transform);
                        iconData.transform.localPosition = Vector3.zero;
                    }

                }
                else if (parentObj.GetComponent<Slot>() == null && childObj != null)
                {

                    Destroy(childObj);
                    iconData.transform.SetParent(this.gameObject.transform);
                    iconData.transform.localPosition = Vector3.zero;
                }
                else if (parentObj.GetComponent<Slot>() != null && childObj == null)
                {
                    iconData.transform.SetParent(this.gameObject.transform);
                    iconData.transform.localPosition = Vector3.zero;
                }
                else if (parentObj.GetComponent<Slot>() != null && childObj != null)
                {
                    childObj.transform.SetParent(parentObj.transform);
                    childObj.transform.localPosition = Vector3.zero;
                    iconData.transform.SetParent(this.gameObject.transform);
                    iconData.transform.localPosition = Vector3.zero;
                }
            }
            else if (iconData.GetComponent<GetItem>() != null)
            {
                if (childObj == null)
                {
                    iconData.transform.SetParent(this.gameObject.transform);
                    iconData.transform.localPosition = Vector3.zero;
                }
                else
                {
                    childObj.transform.SetParent(parentObj.transform);
                    childObj.transform.localPosition = Vector3.zero;
                    iconData.transform.SetParent(this.gameObject.transform);
                    iconData.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    public void RemoveItem()
    {
        if(transform.childCount >0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

}
