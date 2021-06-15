using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcPoint : MonoBehaviour
{
    public Canvas canvas;
    public Camera mainCamera;
    public GameObject targetIndicator;
    public Transform npcTr;
    public TMP_Text text;
    public Transform player;
    private Vector3 npcPos = Vector3.zero;
    private Vector2 halfSize = Vector2.zero;
    

    // Start is called before the first frame update
    void Start()
    {
        halfSize.x = canvas.pixelRect.width * 0.5f;
        halfSize.y = canvas.pixelRect.height * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        npcPos = Camera.main.WorldToScreenPoint(npcTr.position);
        npcPos.x -= halfSize.x;
        npcPos.y -= halfSize.y;
        Vector3 dir = npcTr.position - player.position;
        dir.Normalize();
        Debug.Log(npcPos.x);

        text.text = (Vector3.Distance(player.position, npcTr.position)).ToString("N0") + "m";       
    }
}
