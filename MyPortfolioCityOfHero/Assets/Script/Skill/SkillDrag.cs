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
    //public bool isDrag = false;
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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            eventData.pointerDrag.GetComponent<Button>().enabled = false;
            SkillDataBase skillData = eventData.pointerDrag.GetComponent<SkillDataBase>();

            if (skillData != null)
            {
                if (PlayerSkillData.GetInstance().IsAlreadyLearn(skillData))
                {
                    //isDrag = true;
                    curParentObj = eventData.pointerDrag.transform.parent.gameObject;

                    if (eventData.pointerDrag.GetComponentInParent<Slot>() == null)
                    {
                        skill = Instantiate(eventData.pointerDrag, canvas.transform);
                        skill.transform.position = eventData.pointerDrag.transform.position;
                        skill.GetComponent<Image>().raycastTarget = false;
                        prePos = eventData.pointerDrag.transform.position;
                        eventData.selectedObject = skill;
                    }
                    else
                    {
                        skill = eventData.pointerDrag;
                        skill.transform.SetParent(canvas.transform);
                        skill.transform.position = eventData.position;
                        skill.GetComponent<Image>().raycastTarget = false;
                        prePos = eventData.position;
                        eventData.selectedObject = skill;
                    }
                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
           if (skill != null)
           {
               Vector3 dir = (Vector3)eventData.position - prePos;
               skill.transform.Translate(dir);
               prePos = eventData.position;
           }
         }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
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
            else
            {
                eventData.pointerDrag.GetComponent<Button>().enabled = true;
            }
        }

            //isDrag = false;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


}
