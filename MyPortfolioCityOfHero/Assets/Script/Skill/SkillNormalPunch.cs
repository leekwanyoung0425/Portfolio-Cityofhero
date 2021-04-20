using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillNormalPunch : SkillDataBase
{
    public PlayerAttack attackState;
    private void Start()
    {
        skillName = "Skill_NormalPunch";
        iconImage = gameObject.AddComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_stone_emerge");
    }

    private void Update()
    {
        
    }

    public override void Skillinit()
    {
        attackState.ChangeState(PlayerAttack.STATE.NormalPunch);
    }
}
