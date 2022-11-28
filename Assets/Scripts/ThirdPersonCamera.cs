using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using escapefromtampere.Manager;
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook aimVirtualCam;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GameObject.Find("Player").GetComponent<InputManager>();   
    }

    private void Update()
    {
        if (inputManager.Aim)
        {
            aimVirtualCam.gameObject.SetActive(true);
        }else
        {
            aimVirtualCam.gameObject.SetActive(false);
        }
    }

}
