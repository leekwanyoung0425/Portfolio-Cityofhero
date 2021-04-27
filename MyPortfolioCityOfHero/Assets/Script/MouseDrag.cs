using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler ,IDragHandler, IPointerUpHandler
{
    Vector3 prePos;
    public Canvas canvas;
    List<RaycastResult> skillData;
    GraphicRaycaster gr;
    PointerEventData ped;

    GameObject skill;
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
        ped.position = Input.mousePosition;
        gr.Raycast(ped, skillData);
        skill = Instantiate(skillData[0].gameObject, skillData[0].gameObject.transform);
        skill.transform.position = eventData.position;
        prePos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dir = (Vector3)eventData.position - prePos;
        skill.transform.Translate(dir);
        prePos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("멈춤?");
        skillData.RemoveRange(0, skillData.Count + 1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ped.position = Input.mousePosition;
        gr.Raycast(ped, skillData);

        foreach(RaycastResult obj in skillData)
        {
            Debug.Log(obj.gameObject.name);
        }

        //if(skillData[1].gameObject.GetComponent<Slot>() != null)
        //{
        //    skill.transform.SetParent(skillData[1].gameObject.transform);
        //}
        //else
        //{
        //    Destroy(skill);
        //}
    }
}
