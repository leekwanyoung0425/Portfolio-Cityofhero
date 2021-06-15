using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillBomb : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    RectTransform size;
    public Animator myAnim;
    public GameObject bombEffectSetp1Prefab;
    public GameObject bombEffectSetp2Prefab;
    public Transform bombEffectPos;
    GameObject bombEffect;
    public LayerMask monster;
    public GameObject coolDownEffect;
    private void Start()
    {
        skillNumber = 3;
        isRotateSkill = true;
        skillName = "Skill_Bomb";
        needPrecedingSkillName = "Skill_MagicFire";
        iconImage = GetComponent<Image>();
        //iconImage.sprite = Resources.Load<Sprite>("Icon/S_Green_invade");
        coolDownTime = 5.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
        damage = 60.0f;
        isNonTargetSkill = true;
        skillStep = 3;
        isMoveSkill = false;
    }

    private void Update()
    {
        
    }


    public override void SkillAnim()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Skill_Bomb"))
        {
            bombEffect = Instantiate(bombEffectSetp1Prefab, bombEffectPos.position, Quaternion.identity);
            myAnim.SetTrigger("Skill_Bomb");
            StartCoroutine(InitBombEffectSetp2());
        }
    }

    IEnumerator InitBombEffectSetp2()
    {
        bool stop = true;
        while (stop)
        {
            yield return new WaitForSeconds(1.65f);
            stop = false;
            Destroy(bombEffect);
            bombEffect = Instantiate(bombEffectSetp2Prefab, bombEffectPos.position, Quaternion.identity);
            Collider[] colls = Physics.OverlapSphere(caster.transform.position, 5.0f, monster);
            playerAttack.TargetDamage(colls);
            Destroy(bombEffect, 5.0f);
        }
    }

    public override void CoolDown()
    {
        StartCoroutine(CoolDownInit(coolDownTime));
    }

    IEnumerator CoolDownInit(float coolDownTime)
    {
        size = this.transform.parent.GetComponent<RectTransform>();
        isCoolDown = true;
        size.localScale = new Vector3(0.3f, 0.3f, 0.0f);
        float delta = 0.0f;
        float startTime = 0.0f;
        Color col = iconImage.color;
        col.a = 0.15f;
        iconImage.color = col;
        col.a = 0f;
        float setEffectTime = coolDownTime * 0.6f;
        while (startTime < coolDownTime)
        {
            if (setEffectTime < startTime)
            {
                GameObject obj = Instantiate(coolDownEffect, this.transform.parent);
                obj.GetComponent<CoolDownEffect>().startime = setEffectTime;
                obj.GetComponent<CoolDownEffect>().endtime = coolDownTime;
                obj.GetComponent<CoolDownEffect>().midletime = Mathf.Lerp(setEffectTime, coolDownTime, 0.5f);
                obj.GetComponent<CoolDownEffect>().start = true;
                setEffectTime = coolDownTime;
            }
            startTime += Time.deltaTime;
            delta = 0.7f * Time.deltaTime / coolDownTime;
            col.a = 0.85f * Time.deltaTime / coolDownTime;
            iconImage.color += col;
            size.localScale += new Vector3(delta, delta, 0.0f);
            yield return null;
        }
        size.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        col.a = 1.0f;
        iconImage.color = col;
        isCoolDown = false;
    }

    public override void SkillOnOff()
    {
    }

}
