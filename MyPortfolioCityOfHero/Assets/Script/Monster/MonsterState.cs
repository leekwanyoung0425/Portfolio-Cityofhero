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

    public Animator myAnim;



    // Start is called before the first frame update
    void Start()
    {
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
        InstantiateDamageText(damage);
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

    void InstantiateDamageText(float damage)
    {
        float speed = 1.0f;
        damageTexObj = Instantiate(damageTextPrefab);
        damageTexObj.transform.SetParent(damageTextParent.transform);
        TMP_Text damagetext = damageTexObj.transform.GetComponent<TextMeshPro>();

        bool stop = false;

        while(!stop)
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
        }

    }
}
