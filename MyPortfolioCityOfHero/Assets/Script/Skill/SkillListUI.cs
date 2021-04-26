using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListUI : MonoBehaviour
{
    public List<SkillDataBase> skillUIInLis;
    public int playerSkillPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerSkillPoint = PlayerSkillData.GetInstance().skillPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
