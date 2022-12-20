using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && PlayerInstance.instance.zombiesKilled >= 20)
        {
            SceneManager.LoadScene("BossBattle");
        }
    }
    
        
        
    

}
