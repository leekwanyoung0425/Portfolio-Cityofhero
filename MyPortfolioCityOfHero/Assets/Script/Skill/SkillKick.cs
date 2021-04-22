using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillKick : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    RectTransform size;
    public Animator myAnim;
    public GameObject kickEffectPrefab;
    public Transform kickEffectPos;
    public GameObject kickEffect;
    public TargetSelect targetSelect;
    private void Start()
    {
        isRotateSkill = true;
        skillName = "Skill_Kick";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_divine");
        coolDownTime = 4.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
        damage = 20.0f;
    }

    private void Update()
    {

    }

    public override void SkillAnim()
    {
      kickEffect = Instantiate(kickEffectPrefab, kickEffectPos);
      myAnim.SetTrigger("Skill_Kick");
        kickEffect.GetComponent<DestroySkillEffect>().DestroyEffect(1.18f);
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
        target.GetComponentInChildren<MonsterState>().Damage(damage, caster.transform);
    }

    public override void DamageText()
    {
        target.GetComponentInChildren<MonsterState>().DamageText(damage);
    }
}
