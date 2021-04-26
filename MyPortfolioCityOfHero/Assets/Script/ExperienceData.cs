using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExperienceData : MonoBehaviour
{
    public Dictionary<int, float> experience;

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
        int sum = 50;

        for (int i=0; i<5; i++)
        {
            experience.Add(i + 1, 100 + sum);
            sum = sum + 50;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CheckTargetExperience(int level)
    {
        float exp =0.0f;

        foreach(KeyValuePair<int, float> levelData in experience)
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
        int level = 0;
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
}
