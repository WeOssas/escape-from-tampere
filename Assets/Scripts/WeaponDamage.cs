using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public static WeaponDamage instance;

    public float damageAmount = 10f;
    public float range = 100f;

    public Camera cam;

    private void Awake()
    {
        instance = this;
    }

    

    public void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) 
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damageAmount);
            }
        }
    }
}
