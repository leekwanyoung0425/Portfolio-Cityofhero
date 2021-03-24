using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : CharacterMovement
{

    public LayerMask PickingMask;
    // Start is called before the first frame update
    private void Awake()
    {      
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 1000.0f, PickingMask))
            {
                base.MovePosition(hit.point);
            }
        }
    }
}
