using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnButtonClick : MonoBehaviour
{
    SkillDataBase thisSkillData;
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
        PlayerSkillData.GetInstance().SkillLearn(thisSkillData, this.gameObject);
    }
}
