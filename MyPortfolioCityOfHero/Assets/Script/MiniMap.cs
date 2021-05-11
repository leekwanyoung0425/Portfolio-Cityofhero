using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Camera cam;
    public Transform camTr;
    private Vector3 campos = Vector3.zero;

    public void Setposition(Transform target)
    {
        StartCoroutine(FollowPosition(target));
    }

    IEnumerator FollowPosition(Transform target)
    {
        Vector2 size = new Vector3(300.0f, 300.0f);

        while(target != null)
        {
            Vector3 pos = cam.WorldToViewportPoint(target.position);
            pos.x = pos.x * size.x - size.x / 2;
            pos.y = pos.y * size.y - size.y / 2;
            campos.x = camTr.transform.localPosition.x+pos.x;
            campos.z = camTr.transform.localPosition.z+ pos.y;
            this.transform.localPosition = pos;
            camTr.transform.localPosition = campos;
            yield return null;
        }

    }
}
