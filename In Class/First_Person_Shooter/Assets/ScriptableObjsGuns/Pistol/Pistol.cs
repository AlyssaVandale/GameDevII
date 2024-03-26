using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Interactions;


public class Pistol : Gun
{
    protected override void PrimaryFire()
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
}
