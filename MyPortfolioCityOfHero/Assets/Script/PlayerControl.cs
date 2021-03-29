using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : CharacterMovement
{

    public LayerMask PickingMask;
    BoxCollider _collider;
    float horizontal;
    float vertical;
    private bool isGround = true;
    // Start is called before the first frame update
    private void Awake()
    {       
    }
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        StartCoroutine("IsGroundCheck");
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        CharacterJump();

    }


    IEnumerator IsGroundCheck()
    {
        while(true)
        {
            isGround = Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y+0.1f);

            yield return null;
        }       
    }

    void CharacterMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        base.KeyboardMovePosition(horizontal, vertical);
    }

    void CharacterJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                base.JumpAnimationPlay();
            }
        }
    }
}
