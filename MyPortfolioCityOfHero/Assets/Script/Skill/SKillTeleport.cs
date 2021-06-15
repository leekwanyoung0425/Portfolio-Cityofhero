using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SKillTeleport : SkillDataBase
{
    public PlayerAttack playerAttack;
    public PlayerControl playerControl;
    public Animator myAnim;
    RectTransform size;
    public GameObject coolDownEffect;
    public GameObject skillUseEffect;
    public ParticleSystem teleportEffect;

    private void Awake()
    {
        skillNumber = 7;
        skillName = "Skill_Teleport";
        iconImage = GetComponent<Image>();
        iconImage.sprite = Resources.Load<Sprite>("Icon/S_spell02");
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
        Vector3 pos = caster.transform.position;
        pos.y += 1.2f;
        Instantiate(teleportEffect, pos, caster.transform.rotation);
    }

    public void SkillAnimSetp2()
    {
        StartCoroutine(TeleportAnim());
    }

    public override void CoolDown()
    {
    }

    public override void SkillOnOff()
    {
        playerControl.isTeleportOnOff = !playerControl.isTeleportOnOff;
        if (playerControl.isTeleportOnOff)
            skillUseEffect.SetActive(true);
        else
            skillUseEffect.SetActive(false);
    }

    IEnumerator TeleportAnim()
    {
        yield return new WaitForSeconds(1.0f);
        myAnim.SetTrigger("TeleportMode");
    }
}
