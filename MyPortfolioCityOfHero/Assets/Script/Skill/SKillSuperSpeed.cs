using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SKillSuperSpeed : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    public Animator myAnim;
    RectTransform size;
    public GameObject skillUseEffect;
    public GameObject speedEffect;

    private void Awake()
    {
        skillNumber = 5;
        skillName = "Skill_SuperSpeed";
        iconImage = GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_stranger_light");
        isMoveSkill = true;
    }

    private void Update()
    {
    }



    public override void SkillAnim()
    {
        
    }




    public override void CoolDown()
    {
    }

    public override void SkillOnOff()
    {
        playerControl.isSuperSpeedOnOff = !playerControl.isSuperSpeedOnOff;
        if (playerControl.isSuperSpeedOnOff)
        {
            playerControl.ChangeState(PlayerControl.STATE.RUN);
            speedEffect.SetActive(true);
            skillUseEffect.SetActive(true);
        }
        else
        {
            playerControl.ChangeState(PlayerControl.STATE.IDLE);
            speedEffect.SetActive(false);
            skillUseEffect.SetActive(false);
        }
    }

}
