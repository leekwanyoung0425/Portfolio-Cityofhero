using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.UI;
using TMPro;

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


    public LayerMask targetMask;
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
                break;
            case STATE.TRACE:
                HPCheck();
                break;
            case STATE.ATTACK:
                HPCheck();
                break;
            case STATE.DIE:
                break;
        }
    }

    public void Damage(float damage)
    {
        mydata.GetCurHp -= damage;
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

    IEnumerator PlayerSearch()
    {
            
        while(mystate != STATE.DIE && !isAttacking && !isTracing)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.position, 5.0f, targetMask);

            foreach(Collider player in colls)
            {
                
            }

            yield return null;
        }
    }
}
