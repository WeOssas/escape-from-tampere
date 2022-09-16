using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a GameObject that has health and can take damage.
/// This script should be added to GameObjects that can take damage.
/// </summary>
public class VulnerableObject : MonoBehaviour
{
    /// <summary>
    /// The amount of hitpoints this object has by default.
    /// The health being a non-positive value will result in the death of this object.
    /// </summary>
    public float health = 10;
    
    public void TakeDamage(float amount)
    {
        // TODO: consider a damage animation
        
        // Decrease health
        health -= amount;
        if (health <= 0f)
        {
            // Kill the object.
            Die();
        }
    }
    
    void Die()
    {
        // TODO: consider a death animation
        
        Destroy(gameObject);
    }
}   
