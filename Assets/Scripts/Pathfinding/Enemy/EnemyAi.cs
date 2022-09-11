using System.Linq;
using Pathfinding.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyAi : MonoBehaviour
{
    /* Muista laittaa pelaajaan ja maahn layerit whatIsPlayer ja whatIsGround, sekä laita maahn nav mesh surface.
     ja siihen layerit whatIsPlayer, whatIsEnemy ja whatisGround.*/
    
    public NavMeshAgent agent;

    //Hyökkäys
    /// <summary>
    /// Seconds between the last attack and next possible time the enemy can attack.
    /// </summary>
    [FormerlySerializedAs("timeBetweenAttacks")] public float attackCooldown;
    /// <summary>
    /// Whether the attack is currently cooling down.
    /// </summary>
    private bool attackOnCooldown;
    /// <summary>
    /// Distance the player has to be within to be attacked by this enemy
    /// </summary>
    public float attackRange;

    private AbstractEnemyPathfindGoal[] pathfindingGoals;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        pathfindingGoals = GetComponents<AbstractEnemyPathfindGoal>();
    }

    private void FixedUpdate()
    {
        // Update pathfinding with a new destination if needed.
        if (!agent.hasPath)
        {
            SetPathfindingDestination();
        }
        
        // Attack a nearby player.
        if (Vector3.Distance(transform.position, PlayerInstance.instance.transform.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    private void SetPathfindingDestination()
    {
        // Sorts the goals by priority and filters out the ones with a negative priority.
        var goals =
            from goal in pathfindingGoals
            let priority = goal.GetPriority()
            where priority >= 0
            orderby priority descending
            select goal;
        
        // Goes through all goals and sets the first non-null destination as the destination for the agent.
        foreach (AbstractEnemyPathfindGoal goal in goals)
        {
            NavMeshPath destination = goal.GetDestination();
            if (destination != null)
            {
                agent.SetPath(destination);
                return;
            }
        }
        
        // TODO: Consider a cooldown for searching for a new destination if no destination is found now.
    }

    private void AttackPlayer()
    {
        transform.LookAt(PlayerInstance.instance.transform);

        if (!attackOnCooldown)
        {
            // TODO: Attack the player.
            
            attackOnCooldown = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }
    private void ResetAttack()
    {
        attackOnCooldown = false;
    }
}
