using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiBackUp : MonoBehaviour
{
    /* Muista laittaa pelaajaan ja maahn layerit whatIsPlayer ja whatIsGround, sekä laita maahn nav mesh surface.
     ja siihen layerit whatIsPlayer, whatIsEnemy ja whatisGround.*/

    //-----Perus muuttujat----\\
    
    public int damage;
    
    public int health;
    
    //-------------------------\\

    
    public NavMeshAgent agent;

    public Transform player;

    //Layermaskit tarvitaan maan ja pelaajan erottamiseen
    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;


    //Vartiointi
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Hyökkäys
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Vihollisen "tilat"
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        //Jos pelaaja on näkyvissä ja on hyökkäys alueella
        if(health <= 0)
            Destroy(gameObject);
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();



    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Saavuttu walkpointille
        if (distanceToWalkPoint.magnitude > 1f)
            walkPointSet = false;

    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);    
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    


}
