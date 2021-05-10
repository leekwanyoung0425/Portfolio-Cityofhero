using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class MonsterData : MonoBehaviour
{
    [SerializeField]
    private MonsterDataManager _monsterData;

    //string MonsterName;
    //float MaxHp;
    //float CurHp;
    //Vector3 respawnPos;
    Vector3 respawnDir;
    List<int> DropItems = new List<int>();
    //List<int> holdItemID = new List<int>();

    public string MonsterName { get; private set; }
    public float MaxHp { get; private set; }
    public float CurHp { get; set; }
    public Vector3 respawnPos { get; private set; }
    public List<int> GetDropItems { get { return DropItems; }}
    public float Experience { get; private set; }

    // Start is called before the first frame update
    void Start()
    {      
        MonsterName = _monsterData.MonsterName;
        MaxHp = _monsterData.HP;
        CurHp = _monsterData.HP;
        respawnPos = _monsterData.BaseRespawnPos;
        respawnDir = transform.forward;
        DropItems.Add(1001);
        DropItems.Add(2001);
        Experience = _monsterData.Experience;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

}
