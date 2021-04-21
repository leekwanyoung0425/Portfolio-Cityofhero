using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillNormalPunch : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    public Animator myAnim;
    RectTransform size;
    public TargetSelect targetSelect;
    private void Start()
    {
        skillName = "Skill_NormalPunch";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_stone_emerge");
        coolDownTime = 3.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
    }

    public override void Skillinit()
    {
        playerAttack.ChangeState(PlayerAttack.STATE.NormalPunch);
    }

    public override void SkillAnim()
    {
        if (!myAnim.GetCurrentAnimatorStateInfo(1).IsName("Normal_Punch"))
        {
            myAnim.SetTrigger("Normal_Punch");
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
        damage = 10.0f;
        targetSelect.GetselectTarget.GetComponentInChildren<MonsterState>().Damage(damage, caster.transform);
    }
}
