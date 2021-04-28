using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] slots = new GameObject[5];
   // public List<SkillDataBase> skills = new List<SkillDataBase>();
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
        //SlotRenewal();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //public void SlotRenewal()
    //{
    //    //Debug.Log("리뉴얼");
    //    skills.Clear();
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].transform.childCount > 0)
    //        {
    //            skills.Add(slots[i].transform.GetChild(0).GetComponent<SkillDataBase>());
    //        }
    //    }

    //    //foreach(SkillDataBase test in skills)
    //    //{
    //    //    Debug.Log(test.gameObject.name);
    //    //}
    //}

    public void SkillUseReady(int num, GameObject caster, Transform target)
    {
        playerAttack.skillInit += () => slots[num].GetComponentInChildren<SkillDataBase>().CoolDown();
        playerAttack.skillInit += () => slots[num].GetComponentInChildren<SkillDataBase>().SkillAnim();
        slots[num].GetComponentInChildren<SkillDataBase>().caster = caster;
        slots[num].GetComponentInChildren<SkillDataBase>().target = target;
    }
}
