using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Quest Item", menuName = "Item / QuestItem Data", order  = int.MaxValue)]
public class QuestItem : Item
{
    public override void Remove()
    {
        SelectSlot.GetInstance().curSelectSlot.RemoveItem();
    }

    public override void Use()
    {
        Remove();
    }
}
