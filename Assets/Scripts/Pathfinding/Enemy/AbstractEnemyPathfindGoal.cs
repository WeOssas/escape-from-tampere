using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.Enemy
{
    /// <summary>
    /// An interface allowing for multiple alternate pathfinding schemes to be used.
    /// Each implementing class represents its own goal for the enemy's pathfinding (e.g. idle movement or chasing a player).
    /// Goals should be added to enemies by adding all necessary goals as components (similar to other MonoBehaviour scripts) to the enemy.
    /// </summary>
    public abstract class AbstractEnemyPathfindGoal : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        /// <returns>
        /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
        /// </returns>
        protected abstract Vector3? GetDestinationSuggestion();

        /// <summary>
        /// Attempts to find a destination to be used in pathfinding.
        /// </summary>
        /// <returns>
        /// A valid destination Vector3 or null.
        /// </returns>
        [CanBeNull]
        public NavMeshPath GetDestination()
        {
            int loopCounter = 0;
            // Loop until a something is returned (either a valid destination or null)
            while (true)
            {
                if (loopCounter++ > 10)
                {
                    //Debug.Log("Aborted searching for a new pathfinding destination to prevent an infinitely recurring loop and/or a large impact on performance.");
                    return null;
                }

                // Get a new destination.
                Vector3? destination = GetDestinationSuggestion();

                if (destination != null)
                {
                    // Agent calculates a path to the destination and stores the result into a NavMeshPath.
                    // The path is determined to be valid if it can be calculated (calculation method returns true) and the path is not invalid (is partial or complete).
                    // Valid path will be returned.
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath((Vector3)destination, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        // The path is valid, so return it.
                        return path;
                    }
                }
                else
                {
                    // The suggestion was null, so it is assumed that no suggestion can be provided on next iterations either.
                    return null;
                }
            }
        }

        /// <returns>
        /// The priority of this goal (higher -> more important)
        /// </returns>
        /// <summary>
        /// When the goal is inactive, it should return a negative priority, and a non-negative number when it is active again.
        /// Negative values are considered inactive goals that should never be used.
        /// </summary>
        public abstract int GetPriority();
    }
}