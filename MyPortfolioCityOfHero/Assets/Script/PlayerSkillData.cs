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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillLearn(SkillDataBase skillData, GameObject skillObj)
    {
        if (skillPoint > 0)
        {
            --skillPoint;
            playerSkillData.Add(skillData.skillName, skillData);
            skillObj.GetComponent<Image>().color = new Color(1, 1, 1);
            skillObj.GetComponent<Button>().enabled = false;
        }
    }

}
