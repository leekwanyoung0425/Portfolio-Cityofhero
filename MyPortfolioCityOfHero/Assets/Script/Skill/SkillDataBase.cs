using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillDataBase : MonoBehaviour
{
    public string skillName = "";
    public Image iconImage = null;
    public abstract void Skillinit();
}
