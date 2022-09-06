using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunBullet : MonoBehaviour
{
    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootingForce;

    //stats for the gun
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    
    int bulletsLeft, bulletsShot;

    //Bools
    bool shooting, readyToShoot, reloading;

    //References
    public Camera cam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

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
        //Check if allowed to hold down fire button and take the input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //reloading (automatic reloading not included)
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        
        
        
        
        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
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
        //Find the hit position using raycast
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instiate the bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootingForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        
        //instiate muzzle flash (if you want)
        if(muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        //if more than one bulletspertap make sure to repeat shoot fucntion
        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

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
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
