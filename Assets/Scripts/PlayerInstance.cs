using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to create a static reference to the player.
/// Should be added to the player GameObject.
/// </summary>
public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance instance;
    
    public void Awake()
    {
        instance = this;
    }
}
