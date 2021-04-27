using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillDrag : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler ,IDragHandler, IPointerUpHandler,IBeginDragHandler, IEndDragHandler
{
    Vector3 prePos;
    public Canvas canvas;
    List<RaycastResult> skillData;
    GraphicRaycaster gr;
    PointerEventData ped;
    Transform setParent;
    GameObject skill;
    Image skillImage;
    // Start is called before the first frame update
    void Start()
    {
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        skillData = new List<RaycastResult>();
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
       skill = Instantiate(this.gameObject, canvas.transform);
       skill.transform.position = eventData.position;
       skillImage = skill.GetComponent<Image>();
       skillImage.raycastTarget = false;
       prePos = eventData.position;
       eventData.selectedObject = skill;
        
    }
    public void OnDrag(PointerEventData eventData)
    {
      Vector3 dir = (Vector3)eventData.position - prePos;
      skill.transform.Translate(dir);
      prePos = eventData.position;  
    }

    public void OnEndDrag(PointerEventData eventData)
    {
 
            skillImage.raycastTarget = true;
            Destroy(skill);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


}
