using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    public GameObject Gun;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Gun.GetComponent<Animator>().Play("AimDownSights");
        }

        if (Input.GetMouseButtonUp(1))
        {
            Gun.GetComponent<Animator>().Play("New State");
        }
    }
}
