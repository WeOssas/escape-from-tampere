using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : Interactable
{

    public Item item;
    
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        Debug.Log("You picked up " + item.name + "!");
        bool wasPickedUo = Inventory.Instance.Add(item);
        if(wasPickedUo)
            Destroy(gameObject);

    }
}
