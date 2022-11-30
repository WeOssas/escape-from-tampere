using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public string itemType = null;

    private void Awake()
    {
    }

    public virtual void Use()
    {
        if(itemType == "Health")
        {
            if(name == "Adrenaline")
            {
                PlayerInstance.instance.health += 50;
                Inventory.Instance.Remove(this);
                
            }
                
        }

        
    }
}
