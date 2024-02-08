using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;


public class Player_controller : MonoBehaviour
{
    //Text Variables
    public TMP_Text debug_info;


    //Variables - Camera
    public Camera cam;
    private Vector2 look_input = Vector2.zero;
    private float look_speed = 60f;
    private float horz_look_angle = 0f;
    public bool invert_x = false;
    public bool invert_y = false;
    public int invert_factor_x = 1;
    public int invert_factor_y = 1;
    [Range(0.01f, 1f)] public float sensitivity;

    //Player Variables
    private Vector2 move_input;
    private bool grounded;

    //Move Varaibles
    private CharacterController characterController;
    private Vector3 player_Velocity;
    private Vector3 wish_dir = Vector3.zero;
    public float max_speed = 6f;
    public float accel = 60f;
    public float gravity = 15f;
    public float stop_speed = 0.5f;
    public float jump_pow = 10f;
    public float fric = 4f;


    // Start is called before the first frame update
    void Start()
    {
      //Hide mouse
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      //Invert Camera
      if (invert_x) invert_factor_x = -1;
      if (invert_y) invert_factor_y = -1;

      //for the character controller input 
      characterController = GetComponent<CharacterController>();



    }

    private void FixedUpdate()
    {
        //find wish_dir. 
        wish_dir = transform.right * move_input.x + transform.forward * move_input.y;
        wish_dir = wish_dir.normalized;

        //check if player is on the ground
        grounded = characterController.isGrounded;

        if (grounded)
        {
            player_Velocity = MoveGround(wish_dir, player_Velocity);
        }
        else
        {
            player_Velocity = MoveAir(wish_dir, player_Velocity);
        }

        //Gravity
        player_Velocity.y -= gravity * Time.deltaTime;
        if(grounded && player_Velocity.y < 0)
        {
            player_Velocity.y = -2;
        }




        //player_Velocity = MoveGround(wish_dir, player_Velocity);
        characterController.Move(player_Velocity * Time.deltaTime);


    }

    // Update is called once per frame
    void Update()
    {
        //debugging text
        debug_info.text = "Wish Direction: " + wish_dir.ToString();
        debug_info.text += "\nVelocity: " + player_Velocity.ToString();
        debug_info.text += "\nSpeed: " + new Vector3(player_Velocity.x, 0, player_Velocity.z).ToString();
        debug_info.text += "\nGrounded: " + grounded.ToString();

        Look();
    }

    public void GetLookInput(InputAction.CallbackContext context)
    {
        look_input = context.ReadValue<Vector2>();

    }
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();

    }
    public void GetJumpInput(InputAction.CallbackContext context)
    {
        Jump();

    }

    private void Look()
    {
        //Left and Right
        transform.Rotate(Vector3.up, look_input.x * look_speed * Time.deltaTime * invert_factor_x * sensitivity);

        //up and Down
        float angle = look_input.y * look_speed * Time.deltaTime * invert_factor_y * sensitivity;
        horz_look_angle -= angle;
        horz_look_angle = Mathf.Clamp(horz_look_angle, -90, 180);
        cam.transform.localRotation = Quaternion.Euler(horz_look_angle, 0, 0);

    }

    private void Jump()
    {
        if (grounded)
        {
            player_Velocity.y = jump_pow;
        }
    }

    private Vector3 Accelerate(Vector3 wish_dir, Vector3 current_vel, float accele, float max_speed)
    {
        //Vector 3 projectionof current velocity onto wish direction the speed the player is going
        float proj_speed = Vector3.Dot(current_vel, wish_dir);
        float accel_speed = accele * Time.deltaTime;

        //truncate accelerated velocity if needed
        if(proj_speed + accel_speed > max_speed)
        {
            accel_speed = max_speed - proj_speed;

        }
        //return new speed
        return current_vel + (wish_dir * accel_speed);
    }

    private Vector3 MoveAir(Vector3 wish_dir, Vector3 current_vel)
    {
        return Accelerate(wish_dir, current_vel, accel, max_speed);

    }

    private Vector3 MoveGround(Vector3 wish_dir, Vector3 current_vel)
    {
        //create new velocity vector
        Vector3 new_vel = new Vector3(current_vel.x, 0, current_vel.z); //remove y component

        //since on ground apply friction
        float speed = new_vel.magnitude;
        if (speed <= stop_speed)
        {
            new_vel = Vector3.zero;
            speed = 0;
        }
        if(speed != 0)
        {
            float drop = speed * fric * Time.deltaTime;
            new_vel *= Mathf.Max(speed - drop, 0) / speed;  //scale based on friction

        }
        new_vel = new Vector3(new_vel.x, current_vel.y, new_vel.z); //add y com back in

        return Accelerate(wish_dir, new_vel, accel, max_speed);

    }
}
