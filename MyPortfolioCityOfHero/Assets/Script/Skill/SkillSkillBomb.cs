using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSkillBomb : SkillDataBase
{
    public PlayerAttack attackState;
    private void Start()
    {
        skillName = "Skill_Bomb";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_Green_invade");
    }

    private void Update()
    {
        
    }

    public override void Skillinit()
    {
        attackState.ChangeState(PlayerAttack.STATE.SkillBomb);
    }
}
