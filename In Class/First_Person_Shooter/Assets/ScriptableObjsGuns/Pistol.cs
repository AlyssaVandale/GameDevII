using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Interactions;


public class Pistol : MonoBehaviour
{
    //Variables
    public GunData gun_data;
    public Camera cam;
    private Ray ray;

    private int ammo_in_clip;

    //Debug
    public TMP_Text debug_text;

    //shootin 
    private bool pfires_shoot = false;
    private bool pfires_hold = false;

    private float shoot_delay = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        ammo_in_clip = gun_data.ammo_per_clip;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 10000, Color.green);

        //Debug Text shit
        debug_text.text = "Ammo In Clip: " + ammo_in_clip.ToString();

        //shoot
        PrimaryFire();

        //subtract
        if(shoot_delay > 0.0f) { shoot_delay -= Time.deltaTime; }
    }

    public void GetPrimaryFireInput(InputAction.CallbackContext context)
    {
        //check if pressed inital
        if (context.phase == InputActionPhase.Started)
        {
            pfires_shoot = true;
        }
        //check if gun auto
        if (gun_data.automatic)
        {
            //check if the hold
            if (context.interaction is HoldInteraction && context.phase == InputActionPhase.Performed)
            {
                pfires_hold = true;
            }
        }

        //check if released
        if (context.phase == InputActionPhase.Canceled)
        {
            pfires_shoot = false;
            pfires_hold = false;
        }
    }
    public void GetSecondaryFireInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SecondaryFire();
        }
    }

    private void PrimaryFire()
    {
        if(shoot_delay <= 0)
        {
            if (pfires_shoot || pfires_hold)
            {
                //delay
                shoot_delay = gun_data.pfires_delay;

                pfires_shoot = false;

                //direction
                Vector3 dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.up) * cam.transform.forward;
                dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.right) * dir;

                //raycast shit
                ray = new Ray(cam.transform.position, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, gun_data.range))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green, 0.05f);
                }

                //Ammo
                ammo_in_clip--;
                if (ammo_in_clip <= 0) ammo_in_clip = gun_data.ammo_per_clip;
                print(ammo_in_clip);
            }
        }
        

    }

    private void SecondaryFire()
    {

    }
}
