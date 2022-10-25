using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GunBullet : MonoBehaviour
{
    /// <summary>
    /// Bullet prefab that is spawned when the gun shoots
    /// </summary>
    public GameObject bullet;
    
    public float shootingForce;

    // Sounds
    public AudioSource ShootingSound;
    public AudioSource ReloadingSound;
    public AudioSource ReloadingCaseDropSound;

    //stats for the gun
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    
    int bulletsLeft, bulletsShot;

    //Bools
    bool shooting, readyToShoot, reloading;

    //References
    public Camera cam;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;


    private void Start()
    {
        ShootingSound = GetComponent<AudioSource>();
        ReloadingSound = GetComponent<AudioSource>();
        ReloadingCaseDropSound = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        //Making sure that magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }





    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        // Get input for shooting (whether the button is down, if allowButtonHold, otherwise whether the button is released)
        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        //reloading (automatic reloading not included)
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
            Debug.Log("Reloading sound");
            ReloadingSound.Play();
            ReloadingCaseDropSound.Play();
        }

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        WeaponDamage.instance.Shoot();
        

        // Direction to shoot towards
        Vector3 bulletDirection = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction;
        
        //Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, transform.position, Quaternion.LookRotation(bulletDirection));

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(bulletDirection * shootingForce, ForceMode.Impulse);

        //instantiate muzzle flash (if you want)
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, transform.position, Quaternion.identity);
        }

        // Shooting sound play
        Debug.Log("Shooting sound");
        ShootingSound.Play();

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already)
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }
        
        //if more than one bulletspertap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShots);
        }

    }

    private void ResetShot()
    {
        //allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        reloading = true;
        
        Invoke(nameof(ReloadFinished), reloadTime);
        
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
