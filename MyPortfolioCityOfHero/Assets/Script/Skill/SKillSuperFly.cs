using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SKillSuperFly : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    public Animator myAnim;
    RectTransform size;
    public GameObject coolDownEffect;

    private void Awake()
    {
        skillNumber = 6;
        skillName = "Skill_SuperFly";
        iconImage = GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_Forward_freeze");
        isMoveSkill = true;
    }

    private void Update()
    {
    }



    public override void SkillAnim()
    {
        myAnim.SetBool("FlyMode", playerControl.isGround);
    }


    public override void CoolDown()
    {
    }

    public override void SkillOnOff()
    {
        if (playerControl.isGround)
        {
            playerControl.ChangeState(PlayerControl.STATE.FLYIDLE);
            playerControl.isGround = false;
            playerControl.myrigid.MovePosition(playerControl.transform.position + (Vector3.up * 0.3f));
        }
        else
        {
            playerControl.ChangeState(PlayerControl.STATE.FALLING);
            playerControl.myrigid.useGravity = true;
        }
    }
}
