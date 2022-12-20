using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// Used to create a static reference to the player.
/// Should be added to the player GameObject.
/// </summary>
public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance instance;
    public int health = 100;
    public int maxHealth = 100;
    public int zombiesKilled;
    public TextMeshProUGUI zombiesKilledTxt;
    public TextMeshProUGUI healthTxt;
    
    public void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        if(SceneManager.GetActiveScene().name == "Tampere" & zombiesKilledTxt != null)
            zombiesKilledTxt.text = "zombies killed: "+zombiesKilled+"/20";
        
        healthTxt.text = "HP: " + health + "/" + maxHealth;

    }
    
}
