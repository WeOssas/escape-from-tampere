using UnityEngine;
using Random = UnityEngine.Random;



public class SoldierPatrol : MonoBehaviour
{
    /// <summary>
    /// The X and Z distance within which the enemy will pathfind to.
    /// </summary>
    public static SoldierPatrol instance;
    
    public float pathfindingRange;

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

    /// <returns>
    /// The priority of this goal (higher -> more important)
    /// </returns>
    /// <summary>
    /// When the goal is inactive, it should return a negative priority, and a non-negative number when it is active again.
    /// Negative values are considered inactive goals that should never be used.
    /// </summary>
    public int GetPriority()
    {
        return 0; // Lowest possible priority.
    }

    private void Awake()
    {
        instance = this;
    }
}
        

