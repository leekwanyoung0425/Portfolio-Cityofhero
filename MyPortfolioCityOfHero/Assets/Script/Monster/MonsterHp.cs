using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHp : MonoBehaviour
{
    [SerializeField]
    private MonsterData _monsterData;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHpPos(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }

}
