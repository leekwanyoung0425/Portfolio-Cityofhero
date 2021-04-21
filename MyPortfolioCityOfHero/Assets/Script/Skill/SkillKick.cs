using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillKick : SkillDataBase
{
    public PlayerAttack attackState;
    public float startTime = 0.0f;
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

    public override void CoolDown()
    {
        
    }

    IEnumerator CoolDownInit(float coolDownTime)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.parent.position);
        //this.transform.parent.localPosition
        while (startTime>= coolDownTime)
        {
            
            yield return null;
        }
    }


}
