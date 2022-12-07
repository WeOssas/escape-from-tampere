using System.Linq;
using JetBrains.Annotations;
using Pathfinding.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyAi : MonoBehaviour
{
    /* Muista laittaa pelaajaan ja maahan layerit Player ja Ground, sekä laita maahan NavMeshSurface
     ja siihen layeri Ground.*/
    
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
    public Animator anim;
    public AudioSource hitSound;
    /// <summary>
    /// Distance the player has to be within to be attacked by this enemy
    /// </summary>
    public float attackRange;
    
    /// <summary>
    /// Amount of damage this enemy deals to the player.
    /// </summary>
    [FormerlySerializedAs("damage")] public int attackDamage;

    private AbstractEnemyPathfindGoal[] pathfindingGoals;
    [CanBeNull] private AbstractEnemyPathfindGoal activeGoal;

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
            anim.SetBool("Patrolling", true);
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
                if (goal != activeGoal)
                {
                    if (activeGoal != null) activeGoal.OnUnused(anim);
                    goal.OnUsed(anim);
                    activeGoal = goal;
                }
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
            // Attack the player

            PlayerInstance.instance.health -= attackDamage;
            
            // Play attack animation
            anim.Play("attack");
            hitSound.Play();


            // Set attack cooldown
            attackOnCooldown = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }
    private void ResetAttack()
    {
        attackOnCooldown = false;
    }
}
