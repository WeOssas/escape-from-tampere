using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.Manager;

public class SetPos : MonoBehaviour
{
    public Transform pos;
    public Transform pos2;
    public InputManager inputManager;


    private void Awake()
    {
       
    }

    private void Update()
    {
        if (inputManager.Aim)
        {
            transform.position = new Vector3(pos2.position.x, pos2.position.y, pos2.position.z);
        }
        if(!inputManager.Aim)
        {
            transform.position = new Vector3(pos.position.x, pos.position.y, pos.position.z);
        }

    }
}
