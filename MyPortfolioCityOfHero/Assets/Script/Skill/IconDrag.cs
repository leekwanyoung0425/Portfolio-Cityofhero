using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconDrag : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler ,IDragHandler, IPointerUpHandler,IBeginDragHandler, IEndDragHandler
{
    Vector3 prePos;
    Canvas canvas;
    Transform setParent;
    GameObject icon = null;
    public GameObject curParentObj = null;
    public bool isDrag = false;
    // Start is called before the first frame update

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
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
            SkillDataBase skillData = eventData.pointerDrag.GetComponent<SkillDataBase>();
            GetItem itemData = eventData.pointerDrag.GetComponent<GetItem>();

            if (skillData != null)
            {
                eventData.pointerDrag.GetComponent<Button>().enabled = false;
                curParentObj = eventData.pointerDrag.transform.parent.gameObject;
                if (PlayerSkillData.GetInstance().IsAlreadyLearn(skillData))
                {
                    isDrag = true;
                    if (eventData.pointerDrag.GetComponentInParent<Slot>() == null)
                    {
                        
                        icon = Instantiate(eventData.pointerDrag, canvas.transform);
                        icon.transform.position = eventData.pointerDrag.transform.position;
                        icon.GetComponent<Image>().raycastTarget = false;
                        prePos = eventData.pointerDrag.transform.position;
                        eventData.selectedObject = icon;
                    }
                    else
                    {
                        icon = eventData.pointerDrag;
                        icon.transform.SetParent(canvas.transform);
                        icon.transform.position = eventData.position;
                        icon.GetComponent<Image>().raycastTarget = false;
                        prePos = eventData.position;
                        eventData.selectedObject = icon;
                    }
                }
            }
            else if (itemData != null)
            {
                curParentObj = eventData.pointerDrag.transform.parent.gameObject;
                isDrag = true;
                eventData.pointerDrag.GetComponent<Button>().enabled = false;
                icon = eventData.pointerDrag;
                icon.transform.SetParent(canvas.transform);
                icon.transform.position = eventData.position;
                icon.GetComponent<Image>().raycastTarget = false;
                prePos = eventData.position;
                eventData.selectedObject = icon;                                 
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
           if (icon != null)
           {
               Vector3 dir = (Vector3)eventData.position - prePos;
                icon.transform.Translate(dir);
               prePos = eventData.position;
           }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (icon != null)
            {
                if (icon.transform.parent.GetComponent<Slot>() == null)
                {
                    Destroy(icon);
                }
                else
                {
                    icon.GetComponent<Image>().raycastTarget = true;
                    if(icon.GetComponent<GetItem>()!= null)
                    {
                        eventData.pointerDrag.GetComponent<Button>().enabled = true;
                    }
                }
            }
            else
            {
                if(eventData.pointerDrag.GetComponent<Button>() != null)
                {
                    eventData.pointerDrag.GetComponent<Button>().enabled = true;
                }
            }
        }

        isDrag = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress.transform.parent != null)
        {
            if (eventData.pointerPress.transform.parent.GetComponent<Slot>() != null)
            {
                SelectSlot.GetInstance().curSelectSlot = eventData.pointerPress.transform.parent.GetComponent<Slot>();
            }
        }
    }
}
