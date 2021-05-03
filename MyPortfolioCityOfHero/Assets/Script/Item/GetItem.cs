using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    public Item item;

    void Start()
    {
        GetComponent<Image>().sprite = item.ItemIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
