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
    public GameObject coolDownEffect;
    private void Awake()
    {
        skillNumber = 0;
        isRotateSkill = true;
        skillName = "Skill_NormalPunch";
        iconImage = GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_stone_emerge");
        coolDownTime = 3.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
        damage = 20.0f;
        dist = 2.0f;
        skillStep = 1;
        isMoveSkill = false;
    }

    private void Update()
    {
    }



    public override void SkillAnim()
    {
      myAnim.SetTrigger("Normal_Punch");
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
