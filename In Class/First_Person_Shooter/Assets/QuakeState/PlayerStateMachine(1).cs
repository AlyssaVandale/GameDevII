using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerStateMachine : MonoBehaviour
{
    //Debug
    public TMP_Text debug_text;

    //Player Inputs
    [HideInInspector] public Vector2 move_input;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool jump_button_pressed = false; 

    //Movement Variables
    [HideInInspector] public CharacterController character_controller;
    [HideInInspector] public Vector3 player_velocity;
    [HideInInspector] public Vector3 wish_dir = Vector3.zero;
    




    //State Machine Variables
    private PlayerBaseState current_state;
    public PlayerGroundState ground_state = new PlayerGroundState();
    public PlayerAirState air_state = new PlayerAirState();

    
    // Start is called before the first frame update
    void Start()
    {
        //Get reference to character controller component.
        character_controller = GetComponent<CharacterController>();


        current_state = ground_state;
        current_state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        DebugText();
        current_state.UpdateState(this);
    }

    private void FixedUpdate()
    {
        FindWishDir();
        current_state.FixedUpdateState(this);
        MovePlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        current_state.ExitState(this);
        //current_state = new_state;
        current_state.EnterState(this);

    }

    public void SwitchState(PlayerBaseState cur_state, PlayerBaseState new_state)
    {
        cur_state.ExitState(this);
        current_state = new_state;
        current_state.EnterState(this);
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
    }

    public void DebugText()
    {
        debug_text.text = "Go Dir: " + wish_dir.ToString();
        debug_text.text += "\nPlayer Velocity: " + player_velocity.ToString();
        debug_text.text += "\nSpeed: " + new Vector3(player_velocity.x, 0, player_velocity.z).magnitude.ToString();
        debug_text.text += "\nGrouned: " + grounded.ToString();
        debug_text.text += "\nState: " + current_state.ToString();
    }
    public void FindWishDir()
    {
        wish_dir = transform.right * move_input.x + transform.forward * move_input.y;
    }

    public void MovePlayer()
    {
        character_controller.Move(player_velocity * Time.deltaTime);
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) jump_button_pressed = true;
        if (context.phase == InputActionPhase.Canceled) jump_button_pressed = false;
    }
}
