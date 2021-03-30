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
    TargetSelect _targetSelect;

    // Start is called before the first frame update
    private void Awake()
    {       
    }
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _targetSelect = GetComponent<TargetSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        CharacterJump();
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
                isGround = false;
                base.JumpAnimationPlay();
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        isGround = true;
        Debug.Log(other.gameObject.name);
    }
}
