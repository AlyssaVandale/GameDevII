using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using TMPro;

public class Gun : MonoBehaviour
{
    //Variables
    public GunData gun_data;
    public Camera cam;
    protected Ray ray;

    protected int ammo_in_clip;

    //Debug
    public TMP_Text debug_text;

    //shootin 
    protected bool pfires_shoot = false;
    protected bool pfires_hold = false;

    protected float shoot_delay = 0.0f;


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
        if (shoot_delay > 0.0f) { shoot_delay -= Time.deltaTime; }
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

    protected virtual void PrimaryFire()
    {

    }

    protected virtual void SecondaryFire()
    {

    }
}
