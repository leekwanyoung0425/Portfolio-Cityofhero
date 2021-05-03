using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemSlots = new List<GameObject>();
    public List<Item> getItems = new List<Item>();
    public ItemDB itemDB;
    public GameObject iemPrefab;

    private static ItemManager instance;
    public static ItemManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<ItemManager>();
        }

        return instance;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetItem(List<int> itemID)
    {
        foreach(int ID in itemID)
        {
            foreach(Item data in itemDB.items)
            {
                if(ID == data.ItemID)
                {
                    getItems.Add(data);
                }
            }
        }

        if(getItems.Count >0)
        {
            SetItemInInventory();
        }
    }

    public void SetItemInInventory()
    {
        int index = 0;
        int count = getItems.Count;

        foreach (GameObject slot in itemSlots)
        {
            if (slot.transform.childCount > 0) return;
            else
            {
                GameObject item =  Instantiate(iemPrefab, slot.transform);
                item.transform.localPosition = Vector3.zero;
                item.GetComponent<GetItem>().item = getItems[index];
                index++;
                count--;
                if (count == 0) return;
            }
        }
        getItems.Clear();
    }


}
