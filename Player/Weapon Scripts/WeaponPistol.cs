using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPistol : MonoBehaviour
{

    //Initialize variables
    private float weaponDamage = 1.0f;
    private string weaponDamageType = "bullet";
    private int weaponFireRate = 24;
    private string weaponAmmoType = "bullet";

    private Ray bulletRaycast;
    [SerializeField] private GameObject bulletHoleDecalPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Shoot when player presses the fire button
        if (Input.GetMouseButtonDown(0)){
            Camera camera = GameObject.Find("Player Camera").GetComponent<Camera>();
            bulletRaycast = camera.ScreenPointToRay(Input.mousePosition);
            bulletRaycast.direction = Quaternion.Euler(Random.Range(-1,1), Random.Range(-1,1), 0) * bulletRaycast.direction;
            RaycastHit bulletHit;

            //Do logic depending on what type of object was hit
            if (Physics.Raycast(bulletRaycast, out bulletHit)){
                
                //If hit object was a wall, create a bullet hole decal
                if (bulletHit.collider.tag == "Wall"){
                    var decal = Instantiate(bulletHoleDecalPrefab, bulletHit.point, Quaternion.LookRotation(bulletHit.normal));
                    decal.transform.forward = -bulletHit.normal;
                    decal.transform.localScale *= 0.5f;
                }
            }
        }
    }
}
