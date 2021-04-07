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

[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }

    public Serialization(List<T> target)
    {
        this.target = target;
    }
}

public class MonsterStatus : MonoBehaviour
{

    public List<MonsterStatusData> _MonsterStatusData = new List<MonsterStatusData>();
    public MonsterStatusData Data;


    public void DataInput()
    {
        _MonsterStatusData.Add(new MonsterStatusData(Data.Index,Data.MonsterName,Data.HP,Data.RespawnPos));
    }


    public void SaveJson()
    {        
        string path = Application.dataPath + "/MonsterStatusData.json";
        string jsdata = JsonUtility.ToJson(new Serialization<MonsterStatusData>(_MonsterStatusData));
        File.WriteAllText(path, jsdata);
        Debug.Log(jsdata);
    }


}
