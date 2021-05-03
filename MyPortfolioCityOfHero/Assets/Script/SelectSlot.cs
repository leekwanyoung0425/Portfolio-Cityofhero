using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSlot : MonoBehaviour
{
    public Slot curSelectSlot;

    private static SelectSlot instance;
    public static SelectSlot GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<SelectSlot>();
        }

        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        curSelectSlot = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
