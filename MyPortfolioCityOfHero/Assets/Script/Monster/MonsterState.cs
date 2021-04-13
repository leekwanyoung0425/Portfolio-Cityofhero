using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;


public class MonsterState : MonoBehaviour
{
    public enum STATE
    {
        IDLE, TRACE, ATTACK,GOBACK, DIE
    }

    public enum SUBSTATE
    {
        NOTHING, SEARCHTRACE, ATTACKEDTRACE
    }

    public STATE mystate = STATE.IDLE;
    public SUBSTATE mysubstate = SUBSTATE.NOTHING;

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

    float enemySearchDist = 10.0f;

    Transform attackedTarget = null;
    Transform findTarget = null;

    public NavMeshAgent myAgent;
    Coroutine changeweight = null;
    Coroutine Search = null;

    bool isAttacking = false;
    bool isAttacked = false;
    bool goback = false;


    float attackDist = 1.5f;
    float moveSpeed = 0.0f;

    Transform curAttackTarget = null;
    // Start is called before the first frame update
    void Start()
    {
        halfsize.x = canvas.pixelRect.width / 2.0f;
        halfsize.y = canvas.pixelRect.height / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void StateChange(STATE s)
    {
        //추적스테이트일때는 다시 들어오게끔
        if (mystate == s) return;
        mystate = s;

        switch (mystate)
        {
            case STATE.IDLE:
                if (Search != null) StopCoroutine(Search);
                StartCoroutine(PlayerSearch());
                break;
            case STATE.TRACE:
                break;
            case STATE.ATTACK:
                if (changeweight != null) StopCoroutine(changeweight);
                StartCoroutine(ChangeLayerWeight(1,1.0f,0.5f));
                isAttacking = true;
                break;
            case STATE.GOBACK:
                GoBack();
                break;
            case STATE.DIE:
                myAnim.SetTrigger("Die");
                this.gameObject.layer = 10;
                Destroy(this.gameObject, 5.0f);
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
            case STATE.TRACE:
                HPCheck();
                switch (mysubstate)
                {
                    case SUBSTATE.SEARCHTRACE:
                        SearchTracing(findTarget);
                        break;
                    case SUBSTATE.ATTACKEDTRACE:
                        AttackedTracing(attackedTarget);
                        break;
                }
                break;
            case STATE.GOBACK:
                break;
            case STATE.ATTACK:
                HPCheck();
                break;
            case STATE.DIE:
                break;
        }
    }

    public void Damage(float damage, Transform target)
    {
        mydata.GetCurHp -= damage;
        StartCoroutine(InstantiateDamageText(damage, damageTextPos));
        mysubstate = SUBSTATE.ATTACKEDTRACE;
        attackedTarget = target;
        if(!isAttacked && !goback)
        {
            isAttacked = true;
            StateChange(STATE.TRACE);
        }
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
        float minDist = 10.0f;

        while(mystate == STATE.IDLE)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.position, enemySearchDist, targetMask);

            foreach(Collider player in colls)
            {
                dist = (player.gameObject.transform.position - transform.position).magnitude;

                if(dist <= minDist)
                {
                    minDist = dist;
                    findTarget = player.gameObject.transform;
                }
            }
            if(findTarget != null)
            {               
                StateChange(STATE.TRACE);
                mysubstate = SUBSTATE.SEARCHTRACE;
            }
            yield return null;
        }
    }

    void SearchTracing(Transform target)
    {

        curAttackTarget = target;
        float curSpeed = 0.0f;
        float maxSpeed = 2.0f;

        while (curSpeed < maxSpeed)
        {
            curSpeed = Mathf.Clamp(curSpeed + Time.deltaTime, 0.0f, maxSpeed);
            myAnim.SetFloat("Speed", curSpeed / maxSpeed);
        }

        moveSpeed = curSpeed;
        myAgent.SetDestination(findTarget.position);

        if (myAgent.remainingDistance <= Mathf.Epsilon)
        {
            StateChange(STATE.ATTACK);
        }

        float dist = Vector3.Distance(target.position,this.transform.position);
        if(dist> enemySearchDist)
        {
            StateChange(STATE.GOBACK);
        }
    }

    void AttackedTracing(Transform target)
    {
        
        curAttackTarget = target;

        if (moveSpeed <= Mathf.Epsilon)
        {
            float curSpeed = 0.0f;
            float maxSpeed = 2.0f;

            while (curSpeed < maxSpeed)
            {
                curSpeed = Mathf.Clamp(curSpeed + Time.deltaTime, 0.0f, maxSpeed);
                myAnim.SetFloat("Speed", curSpeed / maxSpeed);
            }

            moveSpeed = curSpeed;
        }

        myAgent.SetDestination(target.position);
        Debug.Log("공격한 플레이어 추적중");

        if (myAgent.remainingDistance <= Mathf.Epsilon)
        {
            Debug.Log("공격한 플레이어에게 도착 공격시작");
            StateChange(STATE.ATTACK);
        }

        float dist = Vector3.Distance(target.position, this.transform.position);
        
        if (dist > enemySearchDist)
        {
            Debug.Log("공격한 플레이어와 거리가 멀어졌다");
            StateChange(STATE.GOBACK);
        }
        
    }

    void GoBack()
    {
        goback = true;
        curAttackTarget = null;
        findTarget = null;
        attackedTarget = null;
        isAttacked = false;
        isAttacking = false;
        Vector3 gobackPosition = mydata.GetPos;
        myAgent.SetDestination(gobackPosition);
        if(myAgent.remainingDistance <= Mathf.Epsilon)
        {
            goback = false;
            StateChange(STATE.IDLE);
        }
    }

    void Attack()
    {

    }

}
