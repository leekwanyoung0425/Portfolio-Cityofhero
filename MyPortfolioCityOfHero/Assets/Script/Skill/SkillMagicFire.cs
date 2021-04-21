using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillMagicFire : SkillDataBase
{
    public PlayerAttack attackState;
    public float startTime = 0.0f;
    private void Start()
    {
        skillName = "Skill_MagicFire";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_Blue_firework");
    }

    private void Update()
    {
        
    }

    public override void Skillinit()
    {
        attackState.ChangeState(PlayerAttack.STATE.SkillMagicFire);
    }
    public override void CoolDown()
    {

    }
    IEnumerator CoolDownInit(float coolDownTime)
    {
        //this.transform.parent.localPosition
        while (startTime >= coolDownTime)
        {

            yield return null;
        }
    }
}
