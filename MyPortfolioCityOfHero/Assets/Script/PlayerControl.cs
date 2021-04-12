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

    BoxCollider _collider;
    float horizontal = 0.0f;
    float vertical = 0.0f;
    private bool isGround = true;

    public TargetSelect targetSelect;
    public PlayerAttack attackState;

    public bool IsDead = false;
    public bool isShortAttackPossible = false;
    public bool isLongAttackPossible = false;
    public float shortAttackDist = 3.0f;
    public float longAttackDist = 15.0f;

    public bool GetIsAttacking { get; set; }

    public Transform headTr;
    public TMPro.TMP_Text textPrefab;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;

    Coroutine attackAlertTextCheck;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        StartCoroutine("IsAttackDistCheck");
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
        GetIsAttacking = false;
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
                IsDead = true;
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
                WalkCheck();
                JumpCheck();
                AttackCheck();
                break;
            case STATE.WALK:
                base.KeyboardMovePosition(horizontal, vertical);
                IdleCheck();
                JumpCheck();
                break;
            case STATE.ATTACK:
                break;
            case STATE.DEAD:
                break;
            case STATE.FLY:
                break;
        }
    }


    void IdleCheck()
    {
        if (horizontal == 0.0f && vertical == 0.0f)
        {
            ChangeState(STATE.IDLE);
        }
    }

    void WalkCheck()
    {
        if (horizontal != 0.0f || vertical != 0.0f)
        {
            ChangeState(STATE.WALK);
        }
    }

    void JumpCheck()
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

    void AttackCheck()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (targetSelect.GetselectTarget != null && isShortAttackPossible && !GetIsAttacking && targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                GetIsAttacking = true;
                ChangeState(STATE.ATTACK);
                attackState.ChangeState(PlayerAttack.STATE.NormalPunch);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isShortAttackPossible, GetIsAttacking));
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (targetSelect.GetselectTarget != null && isShortAttackPossible && !GetIsAttacking && targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                ChangeState(STATE.ATTACK);
                attackState.ChangeState(PlayerAttack.STATE.SkillKick);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isShortAttackPossible, GetIsAttacking));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (targetSelect.GetselectTarget != null && isLongAttackPossible && !GetIsAttacking && targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                ChangeState(STATE.ATTACK);
                attackState.ChangeState(PlayerAttack.STATE.SkillMagicFire);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isLongAttackPossible, GetIsAttacking));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(!GetIsAttacking)
            {
                ChangeState(STATE.ATTACK);
                attackState.ChangeState(PlayerAttack.STATE.SkillBomb);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        isGround = true;
    }

    IEnumerator IsAttackDistCheck()
    {
       float dist = 0.0f;
               
        while (!IsDead)
        {
            Transform target = targetSelect.GetselectTarget;

            if (target != null)
            {
                dist = (this.transform.position - target.position).magnitude;
                isShortAttackPossible = (dist <= shortAttackDist ) ? true : false;
                isLongAttackPossible = (dist <= longAttackDist) ? true : false;
            }
            yield return null;
        }
    }


    IEnumerator AttackAlertText(Transform targetSelect, bool isDistAttackPossible, bool GetIsAttacking)
    {
        Vector3 textpos = Camera.main.WorldToScreenPoint(headTr.position);
        textpos.x -= halfsize.x;
        textpos.y -= halfsize.y;
        TMPro.TMP_Text attackAlertText = Instantiate(textPrefab);
        attackAlertText.transform.SetParent(canvas.transform);
        attackAlertText.transform.localPosition = textpos;

        if (targetSelect == null)
        {
            attackAlertText.text = "선택된 타겟이 없습니다.";
        }
        else if(!isDistAttackPossible)
        {
            attackAlertText.text = "적이 너무 멀리있습니다.";
        }
        else if(GetIsAttacking)
        {
            attackAlertText.text = "현재 공격중입니다.";
        }
        else if(targetSelect.gameObject.layer != LayerMask.NameToLayer("Monster"))
        {
            attackAlertText.text = "공격 대상이 아닙니다.";
        }

        float textUpSpeed = 5.0f;
        bool stop = false;

        while (!stop)
        {
            attackAlertText.transform.Translate(Vector3.up * textUpSpeed * Time.smoothDeltaTime);
            Color col = attackAlertText.color;
            col.a -= Time.deltaTime * 0.5f;
            attackAlertText.color = col;

            if (attackAlertText.color.a <= Mathf.Epsilon)
            {
                Destroy(attackAlertText.gameObject);
                stop = true;
                attackAlertTextCheck = null;
            }

            yield return null;
        }
    }
}
