using System;
using UnityEngine;

namespace Pathfinding.Enemy
{
    [Serializable]
    public class PlayerChaseGoal : AbstractEnemyPathfindGoal
    {
        
        /// <summary>
        /// The distance the player has to be within to be chased.
        /// </summary>
        public int maxChasingDistance = 10;
        
        /// <returns>
        /// A destination suggestion that will be used if it is determined valid. May be null (in which case the suggestion won't be valid).
        /// </returns>
        protected override Vector3? GetDestinationSuggestion()
        {
            Vector3 playerPos = PlayerInstance.instance.transform.position;
            
            // If the player is in range, return player position, else null.
            return Vector3.Distance(transform.position, playerPos) <= maxChasingDistance ? playerPos : null;
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
            return 100;
        }
       
    }
}