using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunV2 : MonoBehaviour
{
    public bool addSpread = true;
        
    public Vector3 spreadAmount = new Vector3(0.1f, 0.1f, 0.1f);

    public ParticleSystem shootingSystem;

    public Transform bulletSpawnAt;

    public ParticleSystem impactPaticleSystem;

    public TrailRenderer bulletTrail;

    public float shootingDelay = 0.5f;

    public LayerMask mask;

    public AudioSource shootingAudio;

    public AudioSource ReloadAudio;

    public int dmg;

    private Animator animator;
    private float lastShot;
    private int bulletsLeft;
    private bool reloading;
    
    public int magazineSize;
    public float reloadTime;
    private Transform cam;


    private TextMeshProUGUI ammunitionDisplay;
   



    private void Awake()
    {
        ammunitionDisplay = GameObject.Find("AmmoLeft").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        cam = GameObject.Find("Maincamera").GetComponent<Transform>();
        bulletsLeft = magazineSize;
    }

    private void Update()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft + " / " + magazineSize);
        
    }

    public void Shoot()
    {

        if(lastShot + shootingDelay < Time.time && !reloading && bulletsLeft > 0)
        {
            animator.SetBool("isShooting", true);
            shootingSystem.Play();
            Vector3 dir = GetDirection();
            Ray ray = new Ray(cam.position, cam.forward);
            //Range should be changed but we'll use maxValue for now with float.MaxValue.
            if (Physics.Raycast(bulletSpawnAt.position, cam.forward, out RaycastHit hit, float.MaxValue, mask))
            {
                if(hit.collider.tag == "Enemy")
                {
                    SoldierAi soldierHit = hit.collider.GetComponent<SoldierAi>();
                    soldierHit.health -= dmg;
                    soldierHit.gotShot = true;
                }
                shootingAudio.Play();
                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnAt.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                lastShot = Time.time;
                bulletsLeft--;
                
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = transform.forward;
        

        if (addSpread)
        {
            dir += new Vector3(Random.Range(-spreadAmount.x, spreadAmount.x),
            Random.Range(-spreadAmount.y, spreadAmount.y),
            Random.Range(-spreadAmount.z, spreadAmount.z)
            );

            dir.Normalize();
        }
        return dir;
    }

    public void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            ReloadAudio.Play();
            reloading = true;

            Invoke(nameof(ReloadFinished), reloadTime);
        }


        

    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = Trail.transform.position;

        while(time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        animator.SetBool("isShooting", false);
        Trail.transform.position = hit.point;
        Instantiate(impactPaticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }







}



