using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to create a static reference to the player.
/// Should be added to the player GameObject.
/// </summary>
public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance instance;
    public int health;
    
    public void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnDestroy()
    {
        // When the player dies, reload the current scene.
        // TODO: maybe add a death screen/animation and/or checkpoints
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
