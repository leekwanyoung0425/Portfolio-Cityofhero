using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterStatus))]
public class MonsterStatusManager : Editor
{
    MonsterStatus _monsterStatus;
    private void OnEnable()
    {
        _monsterStatus = target as MonsterStatus;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();       
        if (GUILayout.Button("DataInput"))
        {
            _monsterStatus.DataInput();
        }

        if (GUILayout.Button("DataSave"))
        {
            _monsterStatus.SaveJson();
        }
        EditorGUILayout.EndHorizontal();
    }

}
