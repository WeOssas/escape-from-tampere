using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAi : MonoBehaviour
{
    // Static is made just in case.
    public static SoldierAi instance;
    

    public Animator anim;
    public NavMeshAgent agent;

    public GameObject gunBullet;
    public Transform shootPos;
    public Transform playerPos;

    private bool inRestrictedArea;
    private int timesToShoot = 5;
    public bool gotShot;
    public float attackRange;
    private bool inAttackRange;
    

    public float pathfindingRange;
    public float maxChasingRange;

    private void AttackOrPatrol()
    {
        //If soldier got shot and player is in attack range
        if (gotShot && inAttackRange)
        {
            ChaseAndShoot();
        }

        //Patrol if soldier haven't been shot or doesn't have a path or player isn't in attack range
        else if (!gotShot && !agent.hasPath && !inAttackRange)
        {
            agent.SetDestination(SearchPath());
            anim.SetBool("Patrol", true);
        }
    }

    public void ChaseAndShoot()
    {

        if (agent.SetDestination(GetChasingSuggestion()) == false)
        {
            gotShot = false;
            inAttackRange = false;
            return;
        }

        transform.LookAt(PlayerInstance.instance.transform);
        for (int i = 0; i < timesToShoot; i++)
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        /*
        Play an attack animation and instantiate a bullet from shooting positioning (named shootPos in hierachy)
        and add force to the bullet aiming to the player's current position.
        */
        
        anim.Play("attack");
        GameObject newBullet = Instantiate(gunBullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(playerPos.transform.position * 100, ForceMode.Impulse);

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
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerInstance.instance.transform.position) <= attackRange)
        {
            inAttackRange = true;
        }

        AttackOrPatrol();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            gotShot = true;
        }
    }

    

    /// <returns>
    /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
    /// </returns>
   

    

    

  

    


}


