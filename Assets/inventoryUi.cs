using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryUi : MonoBehaviour
{
    public Transform itemParent;
    
    Inventory inventory;

    inventorySlot[] slots;
    
    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemParent.GetComponentsInChildren<inventorySlot>();
    }

    
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
