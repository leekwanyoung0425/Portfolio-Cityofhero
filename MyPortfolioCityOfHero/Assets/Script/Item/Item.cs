using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int ItemID = 0;
    public string ItemName = "";
    public Sprite ItemIcon = null;

    public abstract void Remove();
    public abstract void Use();
}
