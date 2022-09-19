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

    private bool inRestrictedArea;
    private int timesToShoot = 5;
    public bool gotShot;

    public float pathfindingRange;

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
        
        if (!agent.hasPath)
        {
            agent.SetDestination(SearchPath());
            ChaseAndShoot();
            anim.SetBool("Patrol", true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
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
        anim.SetBool("GotShot", true);
    }




}


