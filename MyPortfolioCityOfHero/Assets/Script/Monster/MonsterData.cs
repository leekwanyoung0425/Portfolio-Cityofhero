using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class MonsterData : MonoBehaviour
{
    [SerializeField]
    private MonsterDataManager _monsterData;

    string MonsterName;
    float MaxHp;
    float CurHp;
    Vector3 respawnPos;
    Vector3 respawnDir;

    public string GetName { get { return MonsterName; } }
    public float GetMaxHp { get { return MaxHp; }}
    public float GetCurHp { get { return CurHp; } set { CurHp = value; } }
    public Vector3 GetPos { get { return respawnPos; } }

    // Start is called before the first frame update
    void Start()
    {      
        MonsterName = _monsterData.MonsterName;
        MaxHp = _monsterData.HP;
        CurHp = _monsterData.HP;
        respawnPos = _monsterData.BaseRespawnPos;
        respawnDir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

}
