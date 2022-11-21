using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    
    public List<Item> items = new List<Item>();

    private bool inventoryFull = false;


    private void Awake()
    {
        //Making sure there's only 1 inventory found.
        if (Instance != null)
        {
            Debug.Log("MORE THAN 1 INVENTORY!");
            return;
        }
        // If we pass the test
        Instance = this;
    }

    private void Update()
    {
        if(items.Count == 6)
        {
            inventoryFull = true;
        }
        else
        {
            inventoryFull = false;
        }
    }
    public void Add(Item item)
    { 
    /* Add method is used to add items to inventory */
        
        //If item is default, let's not add it to inventory
        if(!item.isDefaultItem & !inventoryFull)
            items.Add(item);
    }
    public void Remove(Item item)
    {
    /* Remove methhod is used to remove items from inventory */
        
        items.Remove(item);
    }


}

