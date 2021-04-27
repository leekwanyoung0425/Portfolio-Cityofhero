using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject skillData;
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
        
        if(skillData.GetComponent<SkillDataBase>() != null)
        {
            skillData.transform.SetParent(this.gameObject.transform);
            skillData.transform.localPosition = Vector3.zero;
        }
        else
        {
            Destroy(eventData.selectedObject);
        }
    }

}
