using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    // Start is called before the first frame update

    public Image[] slots = new Image[5];
    public List<SkillDataBase> skills = new List<SkillDataBase>();
    public PlayerAttack playerAttack;

    private static SlotData instance;
    public static SlotData GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<SlotData>();
        }

        return instance;
    }

    void Start()
    {
        SlotRenewal();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SlotRenewal()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                skills.Add(slots[i].transform.GetChild(0).GetComponent<SkillDataBase>());
            }
        }
    }

    public void SkillUse(int num, GameObject caster)
    {
        playerAttack.skillInit += () => skills[num].GetComponent<SkillDataBase>().CoolDown();
        playerAttack.skillInit += () => skills[num].GetComponent<SkillDataBase>().SkillAnim();
        skills[num].GetComponent<SkillDataBase>().Skillinit();
        skills[num].GetComponent<SkillDataBase>().caster = caster;
    }
}
