using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    // Start is called before the first frame update

    public Image[] slots = new Image[5];
    public SkillDataBase[] skills = new SkillDataBase[5];
    public PlayerAttack attackState;

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
        if(Input.GetKeyDown(KeyCode.T))
        {
            SlotRenewal();
        }
    }

    public void SlotRenewal()
    {
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                skills[i] = slots[i].transform.GetChild(0);
                skills[i]
            }
        }      
    }

    public void SkillUse(int num)
    {
        switch(num)
        {
            case 1:
                if(slotsInObj[num] != null)
                {
                    slots[num].transform.GetComponent<Animator>().SetTrigger("SkillOn");
                    slotsInObj[num].transform.GetComponent<d>
                }
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
}
