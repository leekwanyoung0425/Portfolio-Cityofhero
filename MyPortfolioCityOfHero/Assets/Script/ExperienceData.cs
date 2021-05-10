using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExperienceData : MonoBehaviour
{
    public Dictionary<int, float> experience;
    public bool isLevelUP = false;
    public GameObject levelUpObj;

    private static ExperienceData instance;
    public static ExperienceData GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<ExperienceData>();
        }

        return instance;
    }

    void Start()
    {
        experience = new Dictionary<int, float>();

        float sum = 0.0f;
        float prevNumValue1;
        float prevNumValue2;

        experience.Add(1, 50);
        experience.Add(2, 100);

        for (int i=3; i<10; i++)
        {
            prevNumValue1 = experience[i-2];
            prevNumValue2 = experience[i-1];
            sum = prevNumValue1 + prevNumValue2;
            experience.Add(i, sum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LevelUpEffOn();
    }

    public float CheckTargetExperience(int level)
    {
        float exp =0.0f;

        foreach(KeyValuePair<int,float> levelData in experience)
        {
            if(levelData.Key == level)
            {
                exp = levelData.Value;
                break;
            }
        }
        return exp;
    }

    public int CheckLevel(float curExperience)
    {
        int level = 1;
        foreach (KeyValuePair<int, float> levelData in experience)
        {
            if (curExperience >= levelData.Value)
            {
                level = levelData.Key + 1;
            }
            else
                return level;
        }
        return level;
    }

    public void GetExperience(float experience)
    {
        PlayerData.GetInstance().GetExperience(experience);
    }

    void LevelUpEffOn()
    {
        if(isLevelUP)
        {
            levelUpObj.SetActive(true);
            isLevelUP = false;
        }
    }
}
