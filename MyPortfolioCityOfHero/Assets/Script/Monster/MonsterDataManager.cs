﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster Data",menuName = "ScriptableObject/Monster Data",order = int.MaxValue)]
public class MonsterDataManager : ScriptableObject
{
    public int Index = 0;
    public string MonsterName = "";
    public float HP = 0.0f;
    public float AttackPower = 0.0f;
    public Vector3 BaseRespawnPos = Vector3.zero;
    public float Experience = 0.0f;
}
