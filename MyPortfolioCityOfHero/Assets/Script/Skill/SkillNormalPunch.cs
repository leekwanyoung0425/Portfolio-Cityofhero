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
    private void Start()
    {
        isRotateSkill = true;
        skillName = "Skill_NormalPunch";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_stone_emerge");
        coolDownTime = 3.0f;
        size = this.transform.parent.GetComponent<RectTransform>();
        damage = 10.0f;
        dist = 1.5f;
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
}
