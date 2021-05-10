using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public void Setposition(Transform target)
    {
        StartCoroutine(FollowPosition(target));
    }

    IEnumerator FollowPosition(Transform target)
    {
        Vector2 size = new Vector3(300.0f, 300.0f);

        while(target != null)
        {
            Vector3 pos = Camera.allCameras[1].WorldToViewportPoint(target.position);
            pos.x = pos.x * size.x - size.x / 2;
            pos.y = pos.y * size.y - size.y / 2;
            this.transform.localPosition = pos;
            yield return null;
        }

    }
}
