using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : CharacterMovement
{

    public LayerMask PickingMask;
    float horizontal;
    float vertical;
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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        base.KeyboardMovePosition(horizontal, vertical);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            base.JumpAnimationPlay();
            base.JumpPosition();
        }
    }

}
