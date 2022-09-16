using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageInput : MonoBehaviour
{
    public static DamageInput instance;
    public int healthAmount;
    public string ThisScene;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(healthAmount <= 0)
            RespawnPlayer();
        
    }

    private void RespawnPlayer()
    {
        SceneManager.LoadScene(ThisScene, LoadSceneMode.Single);
    }

}
