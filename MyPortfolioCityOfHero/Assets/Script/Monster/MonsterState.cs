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
        CREATE, IDLE, SEARCHTRACE, ATTACKEDTRACE, ATTACK,GOBACK, DIE
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

    float enemySearchDist = 10.0f;

    Transform attackedTarget = null;
    Transform findTarget = null;

    public NavMeshAgent myAgent;
    Coroutine Destination = null;
    Coroutine Search = null;

    bool isAttacked = false;
    bool goback = false;


    float attackDist = 1.5f;
    float moveSpeed = 0.0f;

    public static Transform curAttackTarget = null;

    float curSpeed = 0.0f;
    float maxSpeed = 1.0f;

    bool rotend = false;
    AnimatorClipInfo[] myAnimClip;
    
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

    void StateChange(STATE s)
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
                Destination = StartCoroutine(SetDestination(attackedTarget, findTarget));
                break;
            case STATE.ATTACKEDTRACE:
                myAgent.SetDestination(attackedTarget.position);
                if (Destination != null) StopCoroutine(Destination);
                Destination = StartCoroutine(SetDestination(attackedTarget, findTarget));
                break;
            case STATE.ATTACK:
                myAnim.SetBool("Attack", true);
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
            case STATE.SEARCHTRACE:
                HPCheck();
                SearchTracing(findTarget);
                break;
            case STATE.ATTACKEDTRACE:
                HPCheck();
                AttackedTracing(attackedTarget);
                break;
            case STATE.GOBACK:
                GoBackEnd();
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
        mydata.GetCurHp -= damage;
        StartCoroutine(InstantiateDamageText(damage, damageTextPos));
        attackedTarget = target;
        if(!isAttacked && !goback)
        {
            isAttacked = true;
            StateChange(STATE.ATTACKEDTRACE);
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
            Debug.Log(curweight);
            yield return null;
        }
    }

    IEnumerator PlayerSearch()
    {
        float dist;

        while(mystate == STATE.IDLE)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.position, enemySearchDist, targetMask);

            foreach(Collider player in colls)
            {
                dist = (player.gameObject.transform.position - transform.position).magnitude;

                if(dist <= enemySearchDist)
                {
                    enemySearchDist = dist;
                    findTarget = player.gameObject.transform;
                }
            }
            if(findTarget != null)
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

        float dist = Vector3.Distance(target.position,this.transform.position);
        if(dist> enemySearchDist)
        {
            StateChange(STATE.GOBACK);
        }
    }


    void AttackedTracing(Transform target)
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

        float dist = Vector3.Distance(target.position, this.transform.position);
        
        if (dist > enemySearchDist)
        {
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
        Vector3 gobackPosition = mydata.GetPos;
        myAgent.SetDestination(gobackPosition);

    }

    void GoBackEnd()
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
                goback = false;
                StateChange(STATE.IDLE);
            }
        }

        if(mydata.GetCurHp < mydata.GetMaxHp)
        {
            float delta = 10.0f * Time.deltaTime;
            if(mydata.GetCurHp + delta >= mydata.GetMaxHp)
            {
                mydata.GetCurHp = mydata.GetMaxHp;
            }
            else
            {
                mydata.GetCurHp += delta;
            }           
        }
    }


    void Attack()
    {
        Vector3 dir = curAttackTarget.position - this.transform.position;
        dir.Normalize();
        float dist = Vector3.Distance(curAttackTarget.position, this.transform.position);

        float rot = Vector3.Dot(dir, this.transform.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;
        myAnimClip = myAnim.GetCurrentAnimatorClipInfo(0);
        if (dist > attackDist)
        {
            myAnim.SetBool("Attack", false);
            
            if (isAttacked && myAnimClip[0].clip.name == "OrcWalk")
            {
                StateChange(STATE.ATTACKEDTRACE);
            }
            else if(!isAttacked && myAnimClip[0].clip.name == "OrcWalk")
            {
                StateChange(STATE.SEARCHTRACE);
            }
        }
        else if (dist > enemySearchDist)
        {
           
            curAttackTarget = null;
            myAnim.SetBool("Attack", false);
            StateChange(STATE.GOBACK);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    IEnumerator SetDestination(Transform attackedTarget, Transform serchTarget)
    {

        while(myAgent.remainingDistance > Mathf.Epsilon && !goback)
        {
            myAnimClip = myAnim.GetCurrentAnimatorClipInfo(0);
            if (attackedTarget != null && myAnimClip[0].clip.name == "OrcWalk")
            {
                
                myAgent.SetDestination(attackedTarget.position);
            }
            else if (serchTarget != null && myAnimClip[0].clip.name == "OrcWalk")
            {
                myAgent.SetDestination(serchTarget.position);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
