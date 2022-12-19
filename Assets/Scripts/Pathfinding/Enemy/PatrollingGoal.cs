using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinding.Enemy
{
    public class PatrollingGoal : AbstractEnemyPathfindGoal
    {
        /// <summary>
        /// The X and Z distance within which the enemy will pathfind to.
        /// </summary>
        public float pathfindingRange;

        /// <returns>
        /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
        /// </returns>
        protected override Vector3? GetDestinationSuggestion()
        {
            // TODO: Implement pathfinding across different Y coordinates.
            float randomX = Random.Range(-pathfindingRange, pathfindingRange);
            float randomZ = Random.Range(-pathfindingRange, pathfindingRange);

            return transform.position + new Vector3(randomX, 0f, randomZ);
        }

        /// <returns>
        /// The priority of this goal (higher -> more important)
        /// </returns>
        /// <summary>
        /// When the goal is inactive, it should return a negative priority, and a non-negative number when it is active again.
        /// Negative values are considered inactive goals that should never be used.
        /// </summary>
        public override int GetPriority()
        {
            return 0; // Lowest possible priority.
        }

        public override void OnUsed(Animator animator)
        {
            base.OnUsed(animator);
            animator.SetBool("Patrolling", true);
        }
        
        public override void OnUnused(Animator animator)
        {
            base.OnUnused(animator);
            animator.SetBool("Patrolling", false);
        }

        public override int GetUpdateFrequency()
        {
            return 1000; // 20 seconds (under normal conditions)
        }
    }
}