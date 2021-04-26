using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public delegate void MouseEventData(PointerEventData data);

public class MouseEvent : MonoBehaviour, 
    IPointerClickHandler, IPointerDownHandler,
    IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    
    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

   
}
