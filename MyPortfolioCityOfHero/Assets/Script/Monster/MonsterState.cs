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
        IDLE, TRACE, ATTACK, DIE
    }

    STATE mystate = STATE.IDLE;

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

    bool isAttacking = false;
    bool isTracing = false;
    public bool isAttacked = false;

    public LayerMask targetMask;

    float enemySearchDist = 10.0f;

    Transform attackedTarget;

    public NavMeshAgent myAgent;
    Coroutine changeweight = null;

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
        if (mystate == s) return;
        mystate = s;

        switch (mystate)
        {
            case STATE.IDLE:
                break;
            case STATE.TRACE:
                break;
            case STATE.ATTACK:
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
                if (isAttacked && attackedTarget != null)
                    AttackedCheck();
                break;
            case STATE.TRACE:
                HPCheck();
                if (isAttacked && attackedTarget != null)
                    AttackedCheck();
                break;
            case STATE.ATTACK:
                HPCheck();
                if (isAttacked && attackedTarget != null)
                    AttackedCheck();
                break;
            case STATE.DIE:
                break;
        }
    }

    public void Damage(float damage, Transform target)
    {
        mydata.GetCurHp -= damage;
        StartCoroutine(InstantiateDamageText(damage, damageTextPos));
        isAttacked = true;
        attackedTarget = target;
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

    void AttackedCheck()
    {
        float curSpeed = 0.0f;
        float maxSpeed = 2.0f; 

        while(curSpeed < maxSpeed)
        {
            curSpeed = Mathf.Clamp(curSpeed + Time.deltaTime, 0.0f, maxSpeed);
            myAnim.SetFloat("Speed", curSpeed / maxSpeed);
        }
        myAgent.SetDestination(attackedTarget.position);
        if(myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            StateChange(STATE.ATTACK);
            if (changeweight != null) StopCoroutine(changeweight);
            changeweight = StartCoroutine(ChangeLayerWeight(1, 1.0f, 0.5f));
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
        Vector3 dir = Vector3.zero;
        float dist;
        float minDist = 10.0f;
        Transform target = null;

        while(!isTracing)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.position, enemySearchDist, targetMask);

            foreach(Collider player in colls)
            {
                dist = (player.gameObject.transform.position - transform.position).magnitude;

                if(dist <= minDist)
                {
                    minDist = dist;
                    target = player.gameObject.transform;
                }
            }
            yield return null;

            dir = (target.position - transform.position).normalized;
        }
    }

    void TargetTracing(Transform target, Vector3 dir)
    {

    }
}
