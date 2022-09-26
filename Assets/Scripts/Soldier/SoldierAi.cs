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

    private Vector3 SearchPath()
    {
        Vector3 newDest = GetDestinationSuggestion();
        return newDest;
    }


    /// <returns>
    /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
    /// </returns>
    public Vector3 GetDestinationSuggestion()
    {
        // TODO: Implement pathfinding across different Y coordinates.
        float randomX = Random.Range(-pathfindingRange, pathfindingRange);
        float randomZ = Random.Range(-pathfindingRange, pathfindingRange);

        return transform.position + new Vector3(randomX, 0f, randomZ);
    }

    public void ChaseAndShoot()
    {
        
        if(agent.SetDestination(GetChasingSuggestion()) == false)
        {
            gotShot = false;
            inAttackRange = false;
            return;
        }

        transform.LookAt(PlayerInstance.instance.transform);
        for(int i = 0; i < timesToShoot; i++)
        {
            shoot();
        }

    }

    Vector3 GetChasingSuggestion()
    {
        Vector3 playerPos = PlayerInstance.instance.transform.position;

        // If the player is in range, return player position, else null.
        return Vector3.Distance(transform.position, playerPos) <= maxChasingRange ? playerPos : Vector3.zero;
    }

    public void shoot()
    {
        anim.Play("attack");
        GameObject newBullet = Instantiate(gunBullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(playerPos.transform.position * 100, ForceMode.Impulse);

    }

    private void AttackOrPatrol()
    {
        if (gotShot && inAttackRange)
        {
            ChaseAndShoot();
        }

        else if (!gotShot && !agent.hasPath && !inAttackRange)
        {
            agent.SetDestination(SearchPath());
            anim.SetBool("Patrol", true);
        }
    }


}


