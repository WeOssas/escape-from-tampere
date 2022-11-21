using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : Interactable
{
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        Debug.Log("You picked up an item!");
        //Add item to inventory (in this case a hand sanitizer)
        Destroy(gameObject);

    }
}
