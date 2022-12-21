using System;
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

        public int loopTimeout = 3;
        
        private Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        /// <returns>
        /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
        /// </returns>
        protected override Vector3? GetDestinationSuggestion()
        {
            // Try a few (loopTimeout) times to find a valid destination.
            for (int i = 0; i < loopTimeout; i++)
            {
                Vector2 direction = Random.insideUnitCircle * pathfindingRange; // A random direction with the magnitude of pathfindingRange.

                Vector3 raycastOrigin = transform.position;
                raycastOrigin.y = collider.bounds.max.y;
                Vector3 raycastDirection = new Vector3(direction.x, collider.bounds.min.y, direction.y);
                
                RaycastHit hit;
                bool result = Physics.Raycast(
                    raycastOrigin,
                    raycastDirection,
                    out hit,
                    pathfindingRange * 4f, // Not a scientific number, but this should be plenty.
                    LayerMask.NameToLayer("Ground") // Only care about hits to ground
                );

                if (result)
                {
                    return hit.point;
                }
            }
            
            // If nothing valid was found, return null and try again next time.
            return null;
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