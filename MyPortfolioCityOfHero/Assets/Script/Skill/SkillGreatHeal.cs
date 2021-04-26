using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGreatHeal : SkillDataBase
{
    // Start is called before the first frame update
    void Start()
    {
        skillName = "Skill_GreatHeal";
        needPrecedingSkillName = "Skill_Kick";
        skillStep = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SkillAnim() { }

    public override void CoolDown() { }
}
