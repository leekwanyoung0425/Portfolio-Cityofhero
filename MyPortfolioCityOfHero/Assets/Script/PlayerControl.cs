using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControl : CharacterMovement
{
    public enum STATE
    {
        IDLE, WALK, ATTACK, DEAD, SELECT, FLY
    }

    public STATE myState = STATE.IDLE;

    public Animator myAnim;
    BoxCollider _collider;
    float horizontal = 0.0f;
    float vertical = 0.0f;
    private bool isGround = true;

    public TargetSelect targetSelect;

    public bool IsDead = false;
    public bool isShortAttackPossible = false;
    public bool isLongAttackPossible = false;
    public float shortAttackDist = 1.5f;
    public float longAttackDist = 15.0f;

    public bool iscoolDownCheck;
    public bool isAttackCheck;

    public Transform headTr;
    public TMPro.TMP_Text textPrefab;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    public SkillDataBase curCastingSkill;

    Coroutine attackAlertTextCheck;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        StartCoroutine("IsAttackDistCheck");
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
        iscoolDownCheck = false;
        isAttackCheck = false;
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
                myAnim.SetTrigger("Dead");
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
        Image slotdata;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotdata = SlotData.GetInstance().slots[0];
            if (targetSelect.GetselectTarget != null && 
                isShortAttackPossible && !iscoolDownCheck &&
                targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster") &&
                !targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().isDead && !isAttackCheck && slotdata!= null)
            {
                iscoolDownCheck = true;
                isAttackCheck = true;
                ChangeState(STATE.ATTACK);
                curCastingSkill = SlotData.GetInstance().skills[0].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUse(0, this.gameObject);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isShortAttackPossible, isAttackCheck, iscoolDownCheck , slotdata));
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotdata = SlotData.GetInstance().slots[1];
            if (targetSelect.GetselectTarget != null &&
                isShortAttackPossible && !iscoolDownCheck &&
                targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster") &&
                !targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().isDead && !isAttackCheck && slotdata != null)
            {
                iscoolDownCheck = true;
                isAttackCheck = true;
                ChangeState(STATE.ATTACK);
                curCastingSkill = SlotData.GetInstance().skills[1].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUse(1, this.gameObject);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isShortAttackPossible, isAttackCheck, iscoolDownCheck, slotdata));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotdata = SlotData.GetInstance().slots[2];
            if (targetSelect.GetselectTarget != null &&
                isLongAttackPossible && !iscoolDownCheck &&
                targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster") &&
                !targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().isDead && !isAttackCheck&& slotdata != null)
            {
                iscoolDownCheck = true;
                isAttackCheck = true;
                ChangeState(STATE.ATTACK);
                curCastingSkill = SlotData.GetInstance().skills[2].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUse(2, this.gameObject);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isLongAttackPossible, isAttackCheck, iscoolDownCheck, slotdata));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotdata = SlotData.GetInstance().slots[3];
            if (!iscoolDownCheck && !isAttackCheck && slotdata != null)
            {
                iscoolDownCheck = true;
                isAttackCheck = true;
                ChangeState(STATE.ATTACK);
                curCastingSkill = SlotData.GetInstance().skills[3].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUse(3, this.gameObject);
            }
            else
            {
                attackAlertTextCheck = StartCoroutine(AttackAlertText(targetSelect.GetselectTarget, isLongAttackPossible, isAttackCheck, iscoolDownCheck, slotdata));
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


    IEnumerator AttackAlertText(Transform targetSelect, bool isDistAttackPossible, bool isAttackCheck, bool iscoolDownCheck, Image slotdata)
    {
        Vector3 textpos = Camera.main.WorldToScreenPoint(headTr.position);
        textpos.x -= halfsize.x;
        textpos.y -= halfsize.y;
        TMPro.TMP_Text attackAlertText = Instantiate(textPrefab);
        attackAlertText.transform.SetParent(canvas.transform);
        attackAlertText.transform.localPosition = textpos;

        if(slotdata == null)
        {
            attackAlertText.text = "슬롯창이 비어있습니다.";
        }
        else if (targetSelect == null)
        {
            attackAlertText.text = "선택된 타겟이 없습니다.";
        }
        else if(!isDistAttackPossible)
        {
            attackAlertText.text = "적이 너무 멀리있습니다.";
        }
        else if(isAttackCheck)
        {
            attackAlertText.text = "현재 공격중입니다.";
        }
        else if(targetSelect.gameObject.layer != LayerMask.NameToLayer("Monster"))
        {
            attackAlertText.text = "공격 대상이 아닙니다.";
        }
        else if (iscoolDownCheck)
        {
            attackAlertText.text = "재사용 대기중입니다.";
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
