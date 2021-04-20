using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    // Start is called before the first frame update

    public Image[] slots = new Image[5];
    public GameObject[] slotsInObj = new GameObject[5];

    int j = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            SlotRenewal();
        }
    }

    public void SlotRenewal()
    {
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Debug.Log(i);
                Transform img = slots[i].transform.GetChild(0);
                 slotsInObj[i] = img.gameObject;
            }
        }      
    }
}
