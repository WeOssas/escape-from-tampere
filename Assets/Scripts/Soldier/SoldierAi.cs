using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAi : MonoBehaviour
{
    
    public static SoldierAi instance;
    public Animator anim;
    
    public GameObject gunBullet;
    public Transform shootPos;

    private bool inRestrictedArea;
    private int timesToShoot = 5;
    public bool gotShot;

    
    void Shoot()
    {
        GameObject newBullet = Instantiate(gunBullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100, ForceMode.Impulse);
    }
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
        
        
        if (gotShot)
        {
            
            
            for (int i = 0; i < timesToShoot; i++)
            {
                Invoke("Shoot", 0f);
            }
            
            gotShot = false;
        }
           
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            gotShot = true;
        }
    }







}
