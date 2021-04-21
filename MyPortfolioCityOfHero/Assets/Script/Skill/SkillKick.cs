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
        skillName = "Skill_Kick";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_divine");
        coolDownTime = 4.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {

    }

    public override void Skillinit()
    {
        playerAttack.ChangeState(PlayerAttack.STATE.SkillKick);
    }

    public override void SkillAnim()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Skill_Kick"))
        {
            kickEffect = Instantiate(kickEffectPrefab, kickEffectPos);
            myAnim.SetTrigger("Skill_Kick");
        }
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
        damage = 20.0f;
        targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().Damage(damage, caster.transform);
    }
}
