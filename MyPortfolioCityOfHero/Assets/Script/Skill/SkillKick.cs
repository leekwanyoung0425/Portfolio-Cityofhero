using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillKick : SkillDataBase
{
    public PlayerAttack attackState;
    private void Start()
    {
        skillName = "Skill_Kick";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_divine");
    }

    private void Update()
    {
        
    }

    public override void Skillinit()
    {
        attackState.ChangeState(PlayerAttack.STATE.SkillKick);
    }
}
