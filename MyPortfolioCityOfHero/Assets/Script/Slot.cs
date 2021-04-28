using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject skillData = null;
    GameObject childObj = null;
    GameObject curParentObj = null;
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
        if(this.gameObject.transform.childCount > 0) childObj = this.gameObject.transform.GetChild(0).gameObject;
        curParentObj = SkillDrag.GetInstance().curParentObj;



        if (skillData.GetComponent<SkillDataBase>() != null)
        {
            if(curParentObj.GetComponent<Slot>() == null && childObj == null)
            {
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
            else if(curParentObj.GetComponent<Slot>() == null && childObj != null)
            {
                Destroy(childObj);
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
            else if(curParentObj.GetComponent<Slot>() != null && childObj == null)
            {
                Debug.Log("슬롯에서 슬롯이동인데 자식없어");
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
            else if (curParentObj.GetComponent<Slot>() != null && childObj != null)
            {
                Debug.Log("슬롯에서 슬롯이동인데 자식있어");
                childObj.transform.SetParent(curParentObj.transform);
                childObj.transform.localPosition = Vector3.zero;
                skillData.transform.SetParent(this.gameObject.transform);
                skillData.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            Destroy(eventData.selectedObject);
        }
    }

}
