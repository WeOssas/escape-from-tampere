using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringCar : MonoBehaviour
{
    public Camera playerCamera;
    public Camera carCamera;
    public MonoBehaviour CarScript;
    public Transform player;
    public AudioSource audioSource;

    public Transform car;
    public GameObject carModel;
    public GameObject playerModel;
    public Transform spawnPoint;
    public GameObject uiObject;

    // Start is called before the first frame update
    void Start()
    {
        CarScript.enabled = false;
        carCamera.enabled = false;
        audioSource.enabled = false;
        uiObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerModel.transform.position - carModel.transform.position).sqrMagnitude < 25.0f)
        {
            
            uiObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerModel.SetActive(false);
                CarScript.enabled = true;
                carCamera.enabled = true;
                audioSource.enabled = true;
                audioSource.Play();
            }
        }
        else
        {
            uiObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            
            player.transform.position = spawnPoint.transform.position;
            playerModel.SetActive(true);
            CarScript.enabled = false;
            carCamera.enabled = false;
            audioSource.enabled = false;
        }
    }

}
