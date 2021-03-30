using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect : MonoBehaviour
{
    public LayerMask Monster;
    public List<GameObject> targetImage;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    Coroutine isTargetingFollow = null;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000.0f, Monster))
            {
                if (isTargetingFollow != null) StopCoroutine(isTargetingFollow);
                isTargetingFollow = StartCoroutine(TargetingFollow(hit.transform));
            }
        }
    }

    IEnumerator TargetingFollow(Transform target)
    {
        Vector3 monstePosition;
        Vector3 targetUIPosition;

        while (target != null)
        {
            monstePosition = target.transform.position;

            targetUIPosition = Camera.main.WorldToScreenPoint(monstePosition);

            targetUIPosition.x -= halfsize.x;
            targetUIPosition.y -= halfsize.y;

            GameObject targetImageLeftUp = Instantiate(targetImage[1]);
            targetImageLeftUp.transform.SetParent(FindObjectOfType<Canvas>().transform);
            targetImageLeftUp.transform.localPosition = targetUIPosition;
            targetImageLeftUp.transform.localPosition += new Vector3(100.0f, 100.0f, 0);

            yield return null;
        }
    }


}
