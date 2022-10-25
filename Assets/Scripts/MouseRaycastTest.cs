using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycastTest : MonoBehaviour
{
    public Camera Camera;
    public Transform rig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target;
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // This is assuming your crosshair is in the middle of the screen
        RaycastHit hit;
        target = ray.GetPoint(10); // Distance we're aiming at, could be something else
        rig.transform.position = target;

    }
}
