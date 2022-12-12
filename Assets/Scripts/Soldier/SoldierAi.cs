using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAi : MonoBehaviour
{
    // Static is made just in case.
    public static SoldierAi instance;

    public int health;

    public Animator anim;
    public NavMeshAgent agent;

    public GameObject gunBullet;
    public Transform shootPos;
    public Transform safePos;

    private bool inRestrictedArea;
    public bool gotShot;
    public float attackRange;
    private bool hasBeenCalled = false;
   
    

    public float pathfindingRange;
    public float maxChasingRange;

    

    public void Shoot()
    {
        /*
        Play an attack animation and instantiate a bullet from shooting positioning (named shootPos in hierachy)
        and add force to the bullet aiming to the player's current position.
        */
        
        anim.Play("attack");
        GameObject newBullet = Instantiate(gunBullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(PlayerInstance.instance.transform.position * 100, ForceMode.Impulse);
        return;

    }

    public Vector3 GetDestinationSuggestion()
    {
        // TODO: Implement pathfinding across different Y coordinates.
        float randomX = Random.Range(-pathfindingRange, pathfindingRange);
        float randomZ = Random.Range(-pathfindingRange, pathfindingRange);

        return transform.position + new Vector3(randomX, 0f, randomZ);
    }

    private Vector3 SearchPath()
    {
        Vector3 newDest = GetDestinationSuggestion();
        return newDest;
    }


    Vector3 GetChasingSuggestion()
    {
        Vector3 playerPos = PlayerInstance.instance.transform.position;

        // If the player is in range, return player position, else null.
        return Vector3.Distance(transform.position, playerPos) <= maxChasingRange ? playerPos : Vector3.zero;
    }

    private void ShootEverySecond()
    {
        hasBeenCalled = true;
        InvokeRepeating("Shoot", 1f, 1f);
    }
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        
        
        if (gotShot)
        {
            if(GetChasingSuggestion() == Vector3.zero)
            {
                CancelInvoke();
                agent.SetDestination(safePos.position);
                gotShot = false;
                hasBeenCalled = false;

            }
            else
            {
                transform.LookAt(PlayerInstance.instance.transform.position);
                agent.SetDestination(GetChasingSuggestion());
                if (!hasBeenCalled)
                {
                    ShootEverySecond();
                }
               
            }
        }
        else if (!gotShot && !agent.hasPath)
        {
            CancelInvoke();
            agent.SetDestination(SearchPath());
            anim.SetBool("Patrol", true);
        }
        
        
    }




    /// <returns>
    /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
    /// </returns>











}


