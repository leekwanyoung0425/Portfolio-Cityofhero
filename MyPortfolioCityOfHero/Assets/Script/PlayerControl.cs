using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : CharacterMovement
{
    public enum STATE
    {
        IDLE, WALK, ATTACK, DEAD, SELECT, FLY
    }

    public STATE myState = STATE.IDLE;

    public LayerMask PickingMask;
    BoxCollider _collider;
    float horizontal = 0.0f;
    float vertical = 0.0f;
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
        StateProcess();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.IDLE:

                break;
            case STATE.WALK:
                break;
            case STATE.ATTACK:
                break;
            case STATE.DEAD:
                break;
            case STATE.FLY:
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.IDLE:
                if (horizontal != 0.0f || vertical != 0.0f)
                {
                    ChangeState(STATE.WALK);
                }
                CharacterJump();
                break;
            case STATE.WALK:
                base.KeyboardMovePosition(horizontal, vertical);
                if(horizontal == 0.0f && vertical == 0.0f)
                {
                    ChangeState(STATE.IDLE);
                }
                CharacterJump();
                break;
            case STATE.ATTACK:
                break;
            case STATE.DEAD:
                break;
            case STATE.FLY:
                break;
        }
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
    }

    
}
