using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class EnteringCar : MonoBehaviour
{
    public float carEnterDistance = 25.0f;
    public Camera carCamera;
    public MonoBehaviour CarScript;
    public AudioSource audioSource;

    public GameObject car;
    public GameObject player;
    public Transform spawnPoint;
    public TextMeshProUGUI guideText;

    private bool isInCar = false;

    // Start is called before the first frame update
    void Start()
    {
        CarScript.enabled = false;
        carCamera.enabled = false;
        audioSource.enabled = false;
        guideText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInCar)
        {
            if (!isInCar && Vector3.Distance(player.transform.position, car.transform.position) < carEnterDistance)
            {
                UpdateGuideText();
                guideText.gameObject.SetActive(true);

                if (!isInCar && Actions.ingame.EnterCar.WasPerformedThisFrame())
                {
                    player.SetActive(false);
                    CarScript.enabled = true;
                    carCamera.enabled = true;
                    audioSource.enabled = true;
                    audioSource.Play();
                    isInCar = true;
                    guideText.gameObject.SetActive(false);
                }
            }
            else
            {
                guideText.gameObject.SetActive(false);
            }
        }
        else if (Actions.ingame.EnterCar.WasPerformedThisFrame())
        {
            player.transform.position = spawnPoint.transform.position;
            player.SetActive(true);
            CarScript.enabled = false;
            carCamera.enabled = false;
            audioSource.enabled = false;
            isInCar = false;
        }
    }

    private void UpdateGuideText()
    {
        guideText.text = "Press " + Actions.ingame.EnterCar.GetBindingDisplayString() + " to enter/exit the car";
    }
}
