using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillDataBase : MonoBehaviour
{
    public string skillName = "";
    public Image iconImage = null;
    public float coolDownTime = 0.0f;
    public float damage = 0.0f;
    public GameObject caster = null;
    public Transform target = null;
    public bool isRotateSkill;
    public float dist = 0.0f;
    public bool isNonTargetSkill = false;
    public abstract void SkillAnim();
    public abstract void CoolDown();
}
