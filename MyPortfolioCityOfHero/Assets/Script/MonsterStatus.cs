using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class MonsterStatusData
{
    public int Index = 0;
    public string MonsterName = "";
    public float HP = 0.0f;
    public Vector3 RespawnPos = Vector3.zero;

    public MonsterStatusData(int index, string monsterName, float hp, Vector3 pos)
    {
        Index = index;
        MonsterName = monsterName;
        HP = hp;
        RespawnPos = pos;
    }
}

public class MonsterStatus : MonoBehaviour
{

    List<MonsterStatusData> _MonsterStatusData = new List<MonsterStatusData>();
    public MonsterStatusData Data;

    public void DataInput()
    {
        _MonsterStatusData.Add(new MonsterStatusData(Data.Index,Data.MonsterName,Data.HP,Data.RespawnPos));
    }


    public void SaveJson()
    {
        Debug.Log(_MonsterStatusData[0].Index);
        Debug.Log(_MonsterStatusData[0].MonsterName);
        Debug.Log(_MonsterStatusData[0].HP);
        Debug.Log(_MonsterStatusData[0].RespawnPos);
        Debug.Log(_MonsterStatusData[1].Index);
        Debug.Log(_MonsterStatusData[1].MonsterName);
        Debug.Log(_MonsterStatusData[1].HP);
        Debug.Log(_MonsterStatusData[1].RespawnPos);
        //string path = Application.dataPath + "/MonsterStatusData.json";
        //string[] jsdata = JsonUtility.ToJson(_MonsterStatusData);
        //File.WriteAllText(path, jsdata);
    }


}
