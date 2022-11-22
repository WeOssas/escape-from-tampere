using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public GameObject inventoryUI;

    public List<Item> items = new List<Item>();

    private bool inventoryFull = false;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;




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
        //USING THE OLD INPUT SYSTET JUST FOR TESTING (CHANGING TO THE NEW ONE LATER)
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }


    public bool Add(Item item)
    {
    /* Add method is used to add items to inventory */

        //If item is default, let's not add it to inventory
        if(!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(item);
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;

    }
    public void Remove(Item item)
    {
    /* Remove methhod is used to remove items from inventory */

        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }


}

