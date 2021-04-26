using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillDataBase : MonoBehaviour
{
    public int skillNumber;
    public string skillName = "";
    public string needPrecedingSkillName = "";
    public Image iconImage = null;
    public float coolDownTime = 0.0f;
    public bool isCoolDown = false;
    public float damage = 0.0f;
    public GameObject caster = null;
    public Transform target = null;
    public bool isRotateSkill;
    public float dist = 0.0f;
    public bool isNonTargetSkill = false;
    public int skillStep = 0;
    public abstract void SkillAnim();
    public abstract void CoolDown();
}
