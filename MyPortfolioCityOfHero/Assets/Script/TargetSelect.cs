using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect : MonoBehaviour
{
    public LayerMask Monster;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
    }

    public void Targeting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 monstePosition;
        Vector3 TargetUIPosition;

        if (Physics.Raycast(ray, out hit, 1000.0f, Monster))
        {
            monstePosition = hit.transform.position;

            TargetUIPosition = Camera.main.WorldToScreenPoint(monstePosition);

            Debug.Log(TargetUIPosition);

        }
    }
}
