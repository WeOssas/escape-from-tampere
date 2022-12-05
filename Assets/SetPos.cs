using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPos : MonoBehaviour
{
    public Transform pos;
    public Transform pos2;


    private void Awake()
    {
       
    }

    private void Update()
    {
        if (Actions.ingame.Aim.WasPerformedThisFrame())
        {
            transform.position = new Vector3(pos2.position.x, pos2.position.y, pos2.position.z);
        }
        else
        {
            transform.position = new Vector3(pos.position.x, pos.position.y, pos.position.z);
        }

    }
}
