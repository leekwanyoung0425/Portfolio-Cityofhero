using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillMagicFire : SkillDataBase
{
    public PlayerAttack attackState;
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
}
