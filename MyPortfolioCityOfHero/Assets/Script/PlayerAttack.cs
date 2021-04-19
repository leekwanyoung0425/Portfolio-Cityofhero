using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Animator myAnim;
    public TargetSelect targetSelect;
    public float rotSpeed = 5.0f;
    public Transform myModel;
    public CameraMove cameramove;

    public float punchDamage = 10.0f;
    public float kickDamage = 20.0f;
    public float magicFireDamage = 30.0f;
    public float bombDamage = 15.0f;

    public GameObject kickEffectPrefab;
    public Transform kickEffectPos;
    public GameObject kickEffect;

    public GameObject magicFireEffectBallPrefab;
    public Transform magicFireLeftPos;
    public Transform magicFireRightPos;

    GameObject magicFireLeftEffect;
    GameObject magicFireRightEffect;

    GameObject magicFireLeftEffect1;
    GameObject magicFireRightEffect1;

    public GameObject magicFireEffectExplodePrefab;
    public GameObject magicFireEffectArrowPrefab;

    public GameObject bombEffectSetp1Prefab;
    public GameObject bombEffectSetp2Prefab;
    public Transform bombEffectPos;
    GameObject bombEffect;

    public LayerMask monster;

    Slider hpbar;
    public enum STATE
    {
        Wait, NormalPunch, SkillKick, SkillMagicFire, SkillBomb
    }

    public STATE myState = STATE.Wait;
    float horizontal = 0.0f;
    float vertical = 0.0f;

    public PlayerControl playerControl;
    Coroutine changeweight = null;
    Coroutine characterRotate = null;

    void Start()
    {       
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

        switch(myState)
        {
            case STATE.Wait:
                if (changeweight != null) StopCoroutine(changeweight);
                changeweight = StartCoroutine(ChangeLayerWeight(1, 0.0f, 0.5f));
                playerControl.ChangeState(PlayerControl.STATE.IDLE);
                break;
            case STATE.NormalPunch:
                AttackReady();
                break;
            case STATE.SkillKick:
                AttackReady();
                break;
            case STATE.SkillMagicFire:
                AttackReady();
                break;
            case STATE.SkillBomb:
                if (changeweight != null) StopCoroutine(changeweight);
                changeweight = StartCoroutine(ChangeLayerWeight(1, 1.0f, 0.5f));
                SkillBomb();
                break;
        }           
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Wait:
                break;
            case STATE.NormalPunch:                                            
                break;
            case STATE.SkillKick:
                break;
            case STATE.SkillMagicFire:
                break;
            case STATE.SkillBomb:
                break;
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

    IEnumerator CharacterRotate(Transform target)
    {
        float rotDirection = 1.0f;
        float delta = 0.0f;

        Vector3 direction = target.position - myModel.position;
        direction.Normalize();
        float rot = Vector3.Dot(direction, myModel.forward);
        rot = Mathf.Acos(rot);
        rot = (rot * 180.0f) / Mathf.PI;

        if(Vector3.Dot(myModel.right, direction) < 0.0f)
        {
            rotDirection = -1.0f;
        }
     
        while (rot > Mathf.Epsilon && target != null)
        {
            delta = rotSpeed * Time.smoothDeltaTime;
            
            if(rot - delta <= Mathf.Epsilon)
            {

                delta -= rot;
         
            }
           rot -= delta;
           myModel.Rotate(myModel.transform.up * delta * rotDirection);

            yield return null;
        }
        cameramove.TurnRight = myModel.localRotation.eulerAngles;


        switch (myState)
        {
            case STATE.NormalPunch:
                NormalPunchInit();
                break;
            case STATE.SkillKick:
                SkillKickInit();
                break;
            case STATE.SkillMagicFire:
                SkillMagicFireInit();
                break;
            case STATE.SkillBomb:
                break;
        }
    }

    public void Damage()
    {
        switch (myState)
        {
            case STATE.NormalPunch:
                targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().Damage(punchDamage,transform.parent);
                break;
            case STATE.SkillKick:
                targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().Damage(kickDamage, transform.parent);
                break;
            case STATE.SkillMagicFire:
                targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().Damage(magicFireDamage, transform.parent);
                break;
            case STATE.SkillBomb:
                break;
        }
    }

    void AttackReady()
    {
        if (changeweight != null) StopCoroutine(changeweight);
        changeweight = StartCoroutine(ChangeLayerWeight(1, 1.0f, 0.5f));
        if (characterRotate != null) StopCoroutine(characterRotate);
        characterRotate = StartCoroutine(CharacterRotate(targetSelect.GetselectTarget));
    }

    void NormalPunchInit()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Normal_Punch"))
        {
            myAnim.SetTrigger("Normal_Punch");
        }
    }

    void SkillKickInit()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Skill_Kick"))
        {
            kickEffect = Instantiate(kickEffectPrefab, kickEffectPos);
            myAnim.SetTrigger("Skill_Kick");
        }
    }

    void SkillMagicFireInit()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Skill_MagicFire"))
        {
            
            magicFireLeftEffect = Instantiate(magicFireEffectBallPrefab, magicFireLeftPos.position, magicFireLeftPos.rotation);
            magicFireRightEffect = Instantiate(magicFireEffectBallPrefab, magicFireRightPos.position, magicFireRightPos.rotation);
            magicFireLeftEffect.transform.SetParent(magicFireLeftPos);
            magicFireRightEffect.transform.SetParent(magicFireLeftPos);

            StartCoroutine(EffectUp(magicFireLeftPos, magicFireRightPos, targetSelect.GetselectTarget));
          
            myAnim.SetTrigger("Skill_MagicFire");
        }
    }

    void SkillBomb()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Skill_Bomb"))
        {
            bombEffect = Instantiate(bombEffectSetp1Prefab, bombEffectPos.position, Quaternion.identity);          
            myAnim.SetTrigger("Skill_Bomb");
        }
    }

    public void InitBombEffectSetp2()
    {
        Destroy(bombEffect);
        bombEffect = Instantiate(bombEffectSetp2Prefab, bombEffectPos.position, Quaternion.identity);
        EnemyDamage();
        Destroy(bombEffect, 5.0f);
    }

    void EnemyDamage()
    {
        Collider[] colls = Physics.OverlapSphere(this.transform.parent.position, 5.0f, monster);

        foreach (Collider monster in colls)
        {
          monster.transform.GetComponentInChildren<MonsterState>().Damage(bombDamage, transform.parent);
        }
    }

    IEnumerator EffectUp(Transform leftEffect, Transform rightEffect, Transform target)
    {
        float heightStep1 = 1.0f;
        float heightStep2 = 2.0f;
        float speed = 1.0f;
        while (leftEffect.position.y < heightStep1)
        {
            leftEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            rightEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);

            yield return null;
        }
        Destroy(magicFireLeftEffect);
        Destroy(magicFireRightEffect);

        magicFireLeftEffect = Instantiate(magicFireEffectExplodePrefab, leftEffect.position, leftEffect.rotation);
        magicFireRightEffect = Instantiate(magicFireEffectExplodePrefab, rightEffect.position, rightEffect.rotation);
        magicFireLeftEffect.transform.SetParent(leftEffect);
        magicFireRightEffect.transform.SetParent(rightEffect);
        while (leftEffect.position.y < heightStep2)
        {
            leftEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);
            rightEffect.Translate(Vector3.up * speed * Time.smoothDeltaTime);
          
            if(magicFireLeftEffect.transform.Find("Ring2") != null && magicFireLeftEffect.transform.Find("Ring2").GetComponent<ParticleSystem>().time > 0.1f && magicFireLeftEffect1 == null)
            {
              magicFireLeftEffect1 = Instantiate(magicFireEffectArrowPrefab, leftEffect.position, leftEffect.rotation);
              magicFireRightEffect1 = Instantiate(magicFireEffectArrowPrefab, rightEffect.position, rightEffect.rotation);
              magicFireLeftEffect1.transform.SetParent(leftEffect);
              magicFireRightEffect1.transform.SetParent(rightEffect);
              magicFireLeftEffect1.transform.LookAt(target);
              magicFireRightEffect1.transform.LookAt(target);              
            }
            yield return null;
        }

        Destroy(magicFireLeftEffect);
        Destroy(magicFireRightEffect);

        StartCoroutine(MovigMagicFire(magicFireLeftEffect1.transform, magicFireRightEffect1.transform, target, leftEffect, rightEffect));
    }

    IEnumerator MovigMagicFire(Transform fire1, Transform fire2, Transform target, Transform leftEffect, Transform rightEffect)
    {

        float speed = 10.0f;
        float delta1 = speed * Time.deltaTime;
        float delta2 = speed * Time.deltaTime;

        while (fire1 != null && fire2!=null && target != null)
        {
            Vector3 dir1 = (target.position + Vector3.up * 1.3f) - fire1.position;
            float dist1 = dir1.magnitude;
            dir1.Normalize();
            Vector3 dir2 = (target.position + Vector3.up * 1.3f) - fire2.position;
            float dist2 = dir2.magnitude;
            dir2.Normalize();

            if (dist1 < delta1 || dist2 < delta2)
            {
                if(dist1 < delta1)
                {
                    delta1 = dist1;
                }
                else if(dist2 < delta2)
                {
                    delta2 = dist2;
                }
                Destroy(fire1.gameObject);
                Destroy(fire2.gameObject);
                leftEffect.localPosition = new Vector3(-1, 0, 0);
                rightEffect.localPosition = new Vector3(1, 0, 0);
                Damage();
            }

            if(fire1 != null) fire1.Translate(dir1 * delta1, Space.World);
            if (fire2 != null) fire2.Translate(dir2 * delta2, Space.World);
            
            yield return null;
        }
    }
}
