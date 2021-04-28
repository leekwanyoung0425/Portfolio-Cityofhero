using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillDrag : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler ,IDragHandler, IPointerUpHandler,IBeginDragHandler, IEndDragHandler
{
    Vector3 prePos;
    public Canvas canvas;
    Transform setParent;
    GameObject skill = null;
    Image skillImage;
    public GameObject curParentObj = null;
    // Start is called before the first frame update

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<SkillDataBase>() != null)
        {
            if (PlayerSkillData.GetInstance().IsAlreadyLearn(this.gameObject.GetComponent<SkillDataBase>()))
            {
                curParentObj = this.gameObject.transform.parent.gameObject;

                if (this.gameObject.GetComponentInParent<Slot>() == null)
                {
                    skill = Instantiate(this.gameObject, canvas.transform);
                    skill.transform.position = eventData.position;
                    skill.GetComponent<Image>().raycastTarget = false;
                    prePos = eventData.position;
                    eventData.selectedObject = skill;
                }
                else
                {
                    skill = this.gameObject;
                    skill.transform.SetParent(canvas.transform);
                    skill.transform.position = eventData.position;
                    skill.GetComponent<Image>().raycastTarget = false;
                    prePos = eventData.position;
                    eventData.selectedObject = skill;
                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            Vector3 dir = (Vector3)eventData.position - prePos;
            skill.transform.Translate(dir);
            prePos = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            if (skill.transform.parent.GetComponent<Slot>() == null)
            {
                Destroy(skill);
            }
            else
            {
                skill.GetComponent<Image>().raycastTarget = true;
            }
        }
 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


}
