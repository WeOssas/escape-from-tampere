using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class continuescripty : MonoBehaviour
{
    public string SceneNimi;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter button pressed");
            SceneManager.LoadScene(SceneNimi);
        }
    }

   
}
