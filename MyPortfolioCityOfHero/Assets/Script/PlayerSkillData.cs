using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillData : MonoBehaviour
{
    public Dictionary<string, SkillDataBase> playerSkillData;
    public int skillPoint = 0;
    public TMP_Text skillPointText;
    public GameObject alert;
    public TMP_Text alertText;
    public SkillDataBase test;
    public SkillListUI skillListUI;


    private static PlayerSkillData instance;

    public static PlayerSkillData GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<PlayerSkillData>();
        }

        return instance;
    }

    void Start()
    {
        playerSkillData = new Dictionary<string, SkillDataBase>();
        skillPoint = 1;
        skillPointText.text = skillPoint.ToString();
        playerSkillData.Add("Skill_NormalPunch", test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillLearn(SkillDataBase skillData, GameObject skillObj)
    {
        bool possible = IsPossibleLearn(skillData);

        if (skillPoint > 0 && possible)
        {
            --skillPoint;
            skillPointText.text = skillPoint.ToString();
            playerSkillData.Add(skillData.skillName, skillData);
            Color col = skillObj.GetComponent<Image>().color;
            col.a = 1;
            skillObj.GetComponent<Image>().color = col;
            skillObj.GetComponent<Button>().enabled = false;
            skillListUI.LineDrawEffect(skillData.skillStep);

        }
        else
        {   if(skillPoint <=0 && possible)
            {
                alertText.text = "스킬 포인트가 부족합니다.";
                alert.SetActive(true);
            }
            else if(!possible)
            {
                alertText.text = "지금은 배울 수 없습니다.";
                alert.SetActive(true);
            }
        }
    }
    
    public bool IsPossibleLearn(SkillDataBase skillData)
    {
        bool possible = false;
        
        foreach(string precedingSkill in playerSkillData.Keys)
        {
            if (precedingSkill == skillData.needPrecedingSkillName) return possible = true;
        }

        return possible;
    }

    public bool IsAlreadyLearn(SkillDataBase skillData)
    {
        bool alreadyLearn = false;

        foreach (string skillName in playerSkillData.Keys)
        {
            if (skillName == skillData.skillName) return alreadyLearn = true;
        }
        return alreadyLearn;
    }
}
