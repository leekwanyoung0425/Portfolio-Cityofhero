using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillNormalPunch : SkillDataBase
{
    public PlayerAttack attackState;
    public float startTime = 0.0f;
    RectTransform size;
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
        attackState.ChangeState(PlayerAttack.STATE.NormalPunch);
    }
    public override void CoolDown()
    {

    }

    IEnumerator CoolDownInit(float coolDownTime)
    {
        size.localScale = new Vector3(0.3f, 0.3f, 0.0f);
         0.7:3.0f = x:time.deltatime;
        x = 0.7 timedeltatime / 3
        while (startTime >= coolDownTime)
        {
           
            yield return null;
        }
    }
}
