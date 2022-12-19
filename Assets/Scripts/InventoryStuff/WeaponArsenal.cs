using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponArsenal : MonoBehaviour
{
    public Transform rightGunBone;
    public Transform leftGunBone;
    public List<Arsernal> arsenal;


    private void Awake()
    {
        
    }

    public void SetArsenal(string name)
    {
        var weapon = arsenal.Find(w => w.name == name);

        if (weapon.name != name)
            return;

        //Tuhotaan Vanha ase
        if (rightGunBone.childCount > 0)
            Destroy(rightGunBone.GetChild(0).gameObject);

        //Luodaan uusi ase
        if(weapon.rightGun != null)
        {
            GameObject newRightGun = (GameObject) Instantiate(weapon.rightGun);
            newRightGun.transform.parent = rightGunBone;
            newRightGun.transform.localPosition = Vector3.zero;
            newRightGun.transform.localRotation = Quaternion.Euler(-90,90,0);
        }
        
            
            
    }
    
    [System.Serializable]
    public struct Arsernal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        
    }



}
