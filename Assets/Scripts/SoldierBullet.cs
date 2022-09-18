using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBullet : MonoBehaviour
{

    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(PlayerInstance.instance.health > 0)
            {
                PlayerInstance.instance.health -= damage;
            }
            
        }
    }
}
