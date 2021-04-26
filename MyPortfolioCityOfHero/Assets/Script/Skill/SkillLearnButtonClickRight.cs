﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnButtonClickRight : MonoBehaviour
{
    SkillDataBase thisSkillData;
    public SkillListUI skillListUi;
    // Start is called before the first frame update
    void Start()
    {
        thisSkillData = GetComponent<SkillDataBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClick()
    {
        skillListUi.LeftRightSelect = 2;
        PlayerSkillData.GetInstance().SkillLearn(thisSkillData, this.gameObject);
    }
}
