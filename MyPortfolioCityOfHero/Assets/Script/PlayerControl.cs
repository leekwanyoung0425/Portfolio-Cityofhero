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
    CapsuleCollider _collider;
    float horizontal = 0.0f;
    float vertical = 0.0f;
    private bool isGround = true;

    public TargetSelect targetSelect;

    public bool IsDead = false;
    public bool isPossibleAttackDist = false;


    public bool iscoolDownCheck;

    public Transform headTr;
    public TMPro.TMP_Text textPrefab;
    Canvas canvas;
    Vector2 halfsize = Vector2.zero;
    public SkillDataBase curCastingSkill;
    public PlayerAttack playerAttack;


    Coroutine attackAlertTextCheck;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        canvas = FindObjectOfType<Canvas>();
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
        iscoolDownCheck = false;
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
                InputNumber();
                JumpCheck();
                break;
            case STATE.WALK:
                base.KeyboardMovePosition(horizontal, vertical);
                IdleCheck();
                JumpCheck();
                break;
            case STATE.ATTACK:
                InputNumber();
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

    void AttackCheck(int inputSlotNum)
    {
        SkillDataBase skilldata;
        skilldata = SlotData.GetInstance().slots[inputSlotNum].GetComponentInChildren<SkillDataBase>();
        float dist = 0.0f;
        
        if (targetSelect.GetselectTarget != null)dist = (this.transform.position - targetSelect.GetselectTarget.position).magnitude;
 
        isPossibleAttackDist = skilldata.dist >= dist ? true : false;


        if (skilldata.isNonTargetSkill)
        {
            if (!iscoolDownCheck)
            {
                ChangeState(STATE.ATTACK);
                iscoolDownCheck = true;
                curCastingSkill = SlotData.GetInstance().skills[inputSlotNum].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUseReady(inputSlotNum, this.gameObject, targetSelect.GetselectTarget);
                playerAttack.SkillInit();
            }
            else
            {
                Debug.Log("안들어오냐");
                if (attackAlertTextCheck != null) StopCoroutine(attackAlertTextCheck);
                attackAlertTextCheck = StartCoroutine(AttackAlertText());
            }
        }
        else
        {
            if (skilldata != null && !iscoolDownCheck && targetSelect.GetselectTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && isPossibleAttackDist)
            {
                ChangeState(STATE.ATTACK);
                iscoolDownCheck = true;
                curCastingSkill = SlotData.GetInstance().skills[inputSlotNum].GetComponent<SkillDataBase>();
                SlotData.GetInstance().SkillUseReady(inputSlotNum, this.gameObject, targetSelect.GetselectTarget);
                if (curCastingSkill.isRotateSkill)
                {
                    playerAttack.AttackReady();
                }
                else
                {
                    playerAttack.SkillInit();
                }
            }
            else
            {
                if (attackAlertTextCheck != null) StopCoroutine(attackAlertTextCheck);
                attackAlertTextCheck = StartCoroutine(AttackAlertText(skilldata, iscoolDownCheck, targetSelect.GetselectTarget.gameObject.layer, isPossibleAttackDist));
            }
        }                     
    }

    void OnCollisionEnter(Collision other)
    {
        isGround = true;
    }

  

    IEnumerator AttackAlertText(SkillDataBase slotdata, bool iscoolDownCheck, LayerMask layer, bool isPossibleAttackDist)
    {
        Vector3 textpos = Camera.main.WorldToScreenPoint(headTr.position);
        textpos.x -= halfsize.x;
        textpos.y -= halfsize.y;
        TMPro.TMP_Text attackAlertText = Instantiate(textPrefab);
        attackAlertText.transform.SetParent(canvas.transform);
        attackAlertText.transform.localPosition = textpos;
        float textUpSpeed = 5.0f;
        bool stop = false;

        if (slotdata == null)
        {
            attackAlertText.text = "슬롯창이 비어있습니다.";
        }
        else if (iscoolDownCheck)
        {
            attackAlertText.text = "재사용 대기중입니다.";
        }
        else if(layer != LayerMask.NameToLayer("Monster"))
        {
            attackAlertText.text = "공격 대상이 아닙니다.";
        }

        else if(!isPossibleAttackDist)
        {
            attackAlertText.text = "적이 너무 멀리있습니다.";
        }


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

    IEnumerator AttackAlertText()
    {
        Vector3 textpos = Camera.main.WorldToScreenPoint(headTr.position);
        textpos.x -= halfsize.x;
        textpos.y -= halfsize.y;
        TMPro.TMP_Text attackAlertText = Instantiate(textPrefab);
        attackAlertText.transform.SetParent(canvas.transform);
        attackAlertText.transform.localPosition = textpos;
        float textUpSpeed = 5.0f;
        bool stop = false;

        attackAlertText.text = "재사용 대기중입니다.";

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

    void InputNumber()
    {
        int num = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            num = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            num = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            num = 2;
        else if(Input.GetKeyDown(KeyCode.Alpha4))
            num = 3;
        else if(Input.GetKeyDown(KeyCode.Alpha5))
            num = 4;

        if (num > -1)
        {
            AttackCheck(num);
        }
    }
}
