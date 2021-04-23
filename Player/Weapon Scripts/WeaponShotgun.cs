using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotgun : MonoBehaviour
{

    //Initialize variables
    private float weaponDamage = 10.0f;
    private string weaponDamageType = "bullet";
    private int weaponFireRate = 64;
    private string weaponAmmoType = "bullet";

    //Spray pattern
    private int bulletPattern = 1;
    private float SPREAD_SIZE = 5.0f;

    //Audio effects
    private AudioSource soundSource;
    public AudioClip soundFire;

    //Bullet raycasts
    private Ray bulletRaycast0;
    private Ray bulletRaycast1;
    private Ray bulletRaycast2;
    private Ray bulletRaycast3;
    private Ray[] rayArray;
    
    [SerializeField] private GameObject bulletHoleDecalPrefab;


    void Start()
    {
        soundSource = GameObject.Find("Player Sound Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot when player presses the fire button
        if (Input.GetMouseButtonDown(0)){
            Shoot();
        }
        if (Input.GetMouseButtonDown(1)){
            Shoot();
            Shoot();
        }

    }

    void Shoot(){
        Camera camera = GameObject.Find("Player Camera").GetComponent<Camera>();

        //Change rays' directions to the camera's direction + an offset
        bulletRaycast0 = camera.ScreenPointToRay(Input.mousePosition); //Middle

        bulletRaycast1 = camera.ScreenPointToRay(Input.mousePosition);
        bulletRaycast1.direction = Quaternion.AngleAxis(SPREAD_SIZE, -transform.right * bulletPattern) * bulletRaycast1.direction; //Up, inverse Pythagorean Theorem = (3 * Mathf.Sqrt(2))

        bulletRaycast2 = camera.ScreenPointToRay(Input.mousePosition);
        bulletRaycast2.direction = Quaternion.AngleAxis(SPREAD_SIZE / 2.0f, transform.right * bulletPattern) * bulletRaycast2.direction; //Down
        bulletRaycast2.direction = Quaternion.AngleAxis(SPREAD_SIZE * Mathf.Sqrt(3) / 2, transform.up) * bulletRaycast2.direction; //Right

        bulletRaycast3 = camera.ScreenPointToRay(Input.mousePosition);
        bulletRaycast3.direction = Quaternion.AngleAxis(SPREAD_SIZE / 2.0f, transform.right * bulletPattern) * bulletRaycast3.direction; //Down
        bulletRaycast3.direction = Quaternion.AngleAxis(SPREAD_SIZE * Mathf.Sqrt(3) / 2, -transform.up) * bulletRaycast3.direction; //Left

        RaycastHit bulletHit;

        rayArray = new Ray[]{bulletRaycast0, bulletRaycast1, bulletRaycast2, bulletRaycast3};

        //For each ray, do logic depending on what type of object was hit
        foreach (Ray r in rayArray){
            if (Physics.Raycast(r, out bulletHit)){
                //If hit object was a wall, create a bullet hole decal
                if (bulletHit.collider.tag == "Wall"){
                    var decal = Instantiate(bulletHoleDecalPrefab, bulletHit.point, Quaternion.LookRotation(bulletHit.normal));
                    decal.transform.forward = -bulletHit.normal;
                    decal.transform.localScale *= 0.5f;
                }
            }
        }

        bulletPattern *= -1;

        soundSource.PlayOneShot(soundFire);

    }
}
