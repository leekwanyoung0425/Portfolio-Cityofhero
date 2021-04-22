using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSkillBomb : SkillDataBase
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
    private void Start()
    {
        isRotateSkill = true;
        skillName = "Skill_Bomb";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_Green_invade");
        coolDownTime = 5.0f;
        size = this.transform.parent.GetComponent<RectTransform>();

    }

    private void Update()
    {
        
    }


    public override void SkillAnim()
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
        Damage();
        Destroy(bombEffect, 5.0f);
    }

    public override void CoolDown()
    {
        StartCoroutine(CoolDownInit(coolDownTime));
    }

    IEnumerator CoolDownInit(float coolDownTime)
    {
        size.localScale = new Vector3(0.3f, 0.3f, 0.0f);
        float delta = 0.0f;
        float startTime = 0.0f;
        while (startTime < coolDownTime)
        {
            startTime += Time.deltaTime;
            delta = 0.7f * Time.deltaTime / coolDownTime;
            size.localScale += new Vector3(delta, delta, 0.0f);
            yield return null;
        }
        size.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        playerControl.iscoolDownCheck = false;
    }
    public override void Damage()
    {
        damage = 15.0f;
        Collider[] colls = Physics.OverlapSphere(caster.transform.position, 5.0f, monster);

        foreach (Collider monster in colls)
        {
            monster.transform.GetComponentInChildren<MonsterState>().Damage(damage, caster.transform);
        }
    }
}
