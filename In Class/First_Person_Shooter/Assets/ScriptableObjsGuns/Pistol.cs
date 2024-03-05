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
    }

    public void GetPrimaryFireInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PrimaryFire();
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
        //raycast shit
        ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, gun_data.range))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.05f);
        }

        //Ammo
        ammo_in_clip--;
        if (ammo_in_clip <= 0) ammo_in_clip = gun_data.ammo_per_clip;
        print(ammo_in_clip);

    }

    private void SecondaryFire()
    {

    }
}
