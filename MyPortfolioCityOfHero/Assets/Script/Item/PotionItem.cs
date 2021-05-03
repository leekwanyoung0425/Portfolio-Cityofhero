using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "PotionItem", menuName = "Item / Potion Data", order  = int.MaxValue)]
public class PotionItem : Item
{

    public float Health = 0;


    public override void Use()
    {
        float maxHp = PlayerData.GetInstance().maxHp;

        if (PlayerData.GetInstance().curHp + Health >= maxHp)
        {
            PlayerData.GetInstance().curHp = maxHp;
            PlayerData.GetInstance().hpBar.value = maxHp;
        }
        else
        {
            PlayerData.GetInstance().curHp += Health;
            PlayerData.GetInstance().hpBar.value += Health;
        }

        Remove();
    }

    public override void Remove()
    {
        SelectSlot.GetInstance().curSelectSlot.RemoveItem();
    }
}
