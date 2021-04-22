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
    public abstract void SkillAnim();
    public abstract void CoolDown();
    public abstract void Damage();
    public abstract void DamageText();
}
