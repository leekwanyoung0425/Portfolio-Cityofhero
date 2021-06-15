using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SKillSuperJump : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    public Animator myAnim;
    RectTransform size;
    public GameObject skillUseEffect;
    public GameObject coolDownEffect;

    private void Awake()
    {
        skillNumber = 6;
        skillName = "Skill_SuperJump";
        iconImage = GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_evilcord_splash");
        isMoveSkill = true;
    }

    private void Update()
    {
    }



    public override void SkillAnim()
    {       
    }

    public void SkillAnimSetp1()
    {
        myAnim.SetTrigger("JumpMode");
    }


    public override void CoolDown()
    {
    }

    public override void SkillOnOff()
    {
        playerControl.isSuperJumpOnOff = !playerControl.isSuperJumpOnOff;
        if(playerControl.isSuperJumpOnOff)
            skillUseEffect.SetActive(true);
        else
            skillUseEffect.SetActive(false);
    }
}
