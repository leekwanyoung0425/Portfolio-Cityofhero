﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;


public class MonsterState : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, SEARCHTRACE, ATTACKEDTRACE, ATTACK,turnback, DIE
    }

    public STATE mystate = STATE.CREATE;

    public MonsterData mydata;
    public GameObject hpbarPrefab;
    public GameObject hpbarObj;
    public Slider myhpbar;
    public GameObject myhpbarParent;

    public GameObject damageTextPrefab;
    public GameObject damageTextParent;
    GameObject damageTexObj;

    public Transform damageTextPos;

    public Animator myAnim;
    public Canvas canvas;

    Vector3 halfsize = Vector3.zero;

    public LayerMask targetMask;

    float enemySearchDist = 15.0f;

    Transform findTarget = null;

    public NavMeshAgent myAgent;
    Coroutine Destination = null;
    Coroutine Search = null;

    public bool isAttacked = false;
    bool turnback = false;


    float attackDist = 1.5f;
    float moveSpeed = 0.0f;

    public Transform curAttackTarget = null;

    float curSpeed = 0.0f;
    float maxSpeed = 1.0f;

    bool rotend = false;

    public float damage = 50.0f;

    public bool isDead = false;
    public bool isAttacking = false;
    public bool againTrace = false;
    
    // Start is called before the first frame update
    void Start()
    {
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
        StateChange(STATE.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();       
    }

    public void StateChange(STATE s)
    {
        if (mystate == s) return;
        mystate = s;
        
        switch (mystate)
        {
            case STATE.IDLE:
                if (Search != null) StopCoroutine(Search);
                Search = StartCoroutine(PlayerSearch());
                break;
            case STATE.SEARCHTRACE:
                myAgent.SetDestination(findTarget.position);
                if (Destination != null) StopCoroutine(Destination);
                Destination = StartCoroutine(SetDestination(findTarget, findTarget.position));
                break;
            case STATE.ATTACKEDTRACE:
                myAgent.SetDestination(curAttackTarget.position);
                if (Destination != null) StopCoroutine(Destination);
                Destination = StartCoroutine(SetDestination(curAttackTarget, curAttackTarget.position));
                break;
            case STATE.ATTACK:
                myAnim.SetBool("Attack", true);
                break;
            case STATE.turnback:
                TurnbackStart();
                break;
            case STATE.DIE:
                MonsterDead();
                break;
        }
    }

    void StateProcess()
    {
        switch (mystate)
        {
            case STATE.IDLE:
                HPCheck();
                break;
            case STATE.SEARCHTRACE:
                HPCheck();
                SearchTracing(findTarget);
                break;
            case STATE.ATTACKEDTRACE:
                HPCheck();
                AttackedTracing(curAttackTarget);
                break;
            case STATE.turnback:
                turnbackEnd();
                break;
            case STATE.ATTACK:
                HPCheck();
                Attack();
                break;
            case STATE.DIE:
                break;
        }
    }

    public void Damage(float damage, Transform target)
    {
        if(!isDead && !turnback)
        { 
            mydata.GetCurHp -= damage;           
            curAttackTarget = target;
            if(!isAttacked && !turnback)
            {
                isAttacked = true;
                StateChange(STATE.ATTACKEDTRACE);
            }
        }
    }

    public void DamageText(float damage)
    {
        StartCoroutine(InstantiateDamageText(damage, damageTextPos));
    }

    public void HPCheck()
    {
        if (mydata.GetCurHp <= Mathf.Epsilon)
        {
            StateChange(STATE.DIE);
        }
    }

    public void InstantiateHPbar()
    {
        hpbarObj = Instantiate(hpbarPrefab);
        hpbarObj.transform.SetParent(myhpbarParent.transform);
        myhpbar = hpbarObj.GetComponent<Slider>();
    }


    IEnumerator InstantiateDamageText(float damage, Transform target)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.position + Vector3.up *0.8f) ;
        pos.x -= halfsize.x;
        pos.y -= halfsize.y;
        float speed = 5.0f;
        damageTexObj = Instantiate(damageTextPrefab);
        damageTexObj.transform.SetParent(damageTextParent.transform);
        damageTexObj.transform.localPosition = pos;
        TMP_Text damagetext = damageTexObj.transform.GetComponent<TextMeshProUGUI>();
        damagetext.text = damage.ToString();

        bool stop = false;

        while (!stop)
        {
            damageTexObj.transform.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            Color col = damagetext.color;
            col.a -= Time.deltaTime * 0.5f;
            damagetext.color = col;

            if (damagetext.color.a <= Mathf.Epsilon)
            {
                Destroy(damagetext.gameObject);
                stop = true;
            }
            yield return null;
        }
    }

    IEnumerator ChangeLayerWeight(int layer, float target, float t)
    {
        float speed = t > Mathf.Epsilon ? 1.0f / t : 1f;
        float curweight = myAnim.GetLayerWeight(layer);
        float dir = target - curweight > 0f ? 1f : -1f;
        float value = Mathf.Abs(target - curweight);

        while (curweight < target - Mathf.Epsilon || curweight > target + Mathf.Epsilon)
        {
            float delta = Time.deltaTime * speed;
            if (value - delta <= Mathf.Epsilon)
            {
                delta = value;
            }
            value -= delta;

            curweight += dir * delta;
            myAnim.SetLayerWeight(layer, curweight);
            yield return null;
        }
    }

    IEnumerator PlayerSearch()
    {
        float dist;
        float radius = enemySearchDist * 0.5f;
        while (mystate == STATE.IDLE)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.parent.position, radius, targetMask);

            foreach(Collider player in colls)
            {
                dist = (player.gameObject.transform.position - transform.parent.position).magnitude;

                if(dist <= radius)
                {
                    enemySearchDist = dist;
                    findTarget = player.gameObject.transform;
                }
            }
            if(findTarget != null && !findTarget.GetComponent<PlayerControl>().IsDead)
            {
                StateChange(STATE.SEARCHTRACE);                
            }
            yield return null;
        }
    }

    void SearchTracing(Transform target)
    {

        curAttackTarget = target;

        if (moveSpeed < maxSpeed)
        {
            curSpeed = Mathf.Clamp(curSpeed + Time.deltaTime, 0.0f, maxSpeed);
            moveSpeed = curSpeed;
            myAnim.SetFloat("Speed", curSpeed / maxSpeed);
        }

       
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            StateChange(STATE.ATTACK);
        }

        float dist = Vector3.Distance(target.position,this.transform.parent.position);
        if(dist> enemySearchDist || target.GetComponent<PlayerControl>().IsDead)
        {
            StateChange(STATE.turnback);
        }
    }


    void AttackedTracing(Transform target)
    {
        
        if (moveSpeed < maxSpeed)
        {
            curSpeed = Mathf.Clamp(curSpeed + Time.deltaTime, 0.0f, maxSpeed);
            moveSpeed = curSpeed;
            myAnim.SetFloat("Speed", curSpeed / maxSpeed);
        }
       

        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            StateChange(STATE.ATTACK);
        }

        float dist = Vector3.Distance(target.position, this.transform.parent.position);
        
        if (dist > enemySearchDist || target.GetComponent<PlayerControl>().IsDead)
        {
            StateChange(STATE.turnback);
        }       
    }

    void TurnbackStart()
    {
        turnback = true;
        curAttackTarget = null;
        findTarget = null;
        isAttacked = false;
        Vector3 turnbackPosition = mydata.GetPos;
        myAgent.stoppingDistance = 0.0f;
        myAgent.SetDestination(turnbackPosition);
    }

    void turnbackEnd()
    {
      
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            if (moveSpeed > Mathf.Epsilon)
            {
                curSpeed = Mathf.Clamp(curSpeed - Time.deltaTime, 0.0f, maxSpeed);
                moveSpeed = curSpeed;
                myAnim.SetFloat("Speed", curSpeed / maxSpeed);
            }
            else
            {
                myAgent.stoppingDistance = 1.5f;
                turnback = false;
                StateChange(STATE.IDLE);
            }
        }

        if (mystate == STATE.turnback)
        {
            if (mydata.GetCurHp < mydata.GetMaxHp)
            {
                float delta = 10.0f * Time.deltaTime;
                if (mydata.GetCurHp + delta >= mydata.GetMaxHp)
                {
                    mydata.GetCurHp = mydata.GetMaxHp;
                }
                else
                {
                    mydata.GetCurHp += delta;
                }
            }
        }
        else
        {
            mydata.GetCurHp = mydata.GetMaxHp;
        }
    }


    void Attack()
    {
        Vector3 dir = curAttackTarget.position - this.transform.parent.position;
        dir.Normalize();
        float dist = Vector3.Distance(curAttackTarget.position, this.transform.parent.position);
        isAttacking = true;
        againTrace = false;
        if (!curAttackTarget.GetComponent<PlayerControl>().IsDead)
        {
            if (dist > attackDist)
            {
                myAnim.SetBool("Attack", false);

                if (isAttacked)
                {
                    isAttacking = false;
                    againTrace = true;
                }
                else
                {
                    isAttacking = false;
                    againTrace = true;
                }
            }
            else if (dist > enemySearchDist)
            {
                isAttacking = false;
                curAttackTarget = null;
                myAnim.SetBool("Attack", false);
                StateChange(STATE.turnback);
            }
            else
            {
                transform.parent.rotation = Quaternion.LookRotation(dir);
            }
        }
        else
        {
            isAttacking = false;
            curAttackTarget = null;
            myAnim.SetBool("Attack", false);
            StateChange(STATE.turnback);
        }
    }

    IEnumerator SetDestination(Transform Target, Vector3 pos)
    {
        while (!isAttacking && !turnback && !isDead)
        {
            if (pos != Target.position)
            {
                pos = Target.position;               
                if (Target != null)
                {
                    myAgent.SetDestination(pos);
                }
            }
            yield return null;
        }
    }

    public void DamageToAttacked()
    {
        if(curAttackTarget != null)
        {
            curAttackTarget.GetComponent<PlayerData>().Damage(damage);
        }
    }

    public void MonsterDead()
    {
        myAnim.SetTrigger("Die");
        isDead = true;
        this.transform.parent.gameObject.layer = 10;
        Destroy(this.transform.parent.gameObject, 5.0f);
    }
}
