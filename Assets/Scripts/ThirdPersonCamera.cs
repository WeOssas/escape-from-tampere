using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using escapefromtampere.Manager;
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCam;
    private InputManager inputManager;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }
    private void Update()
    {
        if (inputManager.Aim)
        {
            aimCam.gameObject.SetActive(true);
        }
        else
        {
            aimCam.gameObject.SetActive(false);
        }
    }



}
