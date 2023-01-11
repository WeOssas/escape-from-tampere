using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public Transform from;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = from.position;
    }
}
