using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
// Private Variables
    private CharacterController PlayerController;
    private float MouseX, MouseY;
    private float MoveFrontBack, MoveLeftRight;
    private Animator PlayerAni;
    private bool Running = false;
    private bool Walking = true;

    // Stats
    private int Health = 2;

// Public Variables
    public Transform playercam, character, centerpoint;
    public float MouseSensitivity = 10.0f;
    public float MovementSpeed = 2.0f;
    //public float CameraZoom = -2.5f;
    public float RotationSpeed = 25;
    public float Gravity = 9.8f;
    public float jumpStrenght = 4;
    public float Vertical_velocity;
    public bool IsAlive = true;

// Initialization
    void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        PlayerAni = GetComponent<Animator>();
        Cursor.visible = false;

    }

// Update Game
    void Update()
    {
        if (IsAlive)
        {
            Movement();
        }
        if (!IsAlive)
        {
            transform.position = new Vector3(0, 0, 0);
            IsAlive = true;
        }
    }


    void Movement()
    {

// Camera Movement
        MouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        MouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        #region Jump
        //if ((Input.GetButtonDown("Fire3") && Player.GetComponent<cameraswitch>().is_player))
        //{
        //    Debug.Log("sprint");
        //    MovementSpeed = 10;
        //}
        //else if (Input.GetButtonUp("Fire3") && Player.GetComponent<cameraswitch>().is_player)
        //{
        //    Debug.Log("sprint");
        //    MovementSpeed = 5;
        //}
        #endregion

        #region Crouch
        //if (Input.GetKeyDown(KeyCode.LeftControl) && Player.GetComponent<cameraswitch>().is_player)
        //{
        //    MovementSpeed = 2.5f;
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftControl) && Player.GetComponent<cameraswitch>().is_player)
        //{
        //    MovementSpeed = 5;
        //}
        #endregion

        MouseY = Mathf.Clamp(MouseY, -35f, 35f);
        //playercam.LookAt(centerpoint);
        centerpoint.localRotation = Quaternion.Euler(MouseY, MouseX, 0);

        if (PlayerController.isGrounded)
        {
            Vertical_velocity = -Gravity * Time.deltaTime;
            if (Input.GetButton("Jump"))
            {
                Vertical_velocity = jumpStrenght;
                PlayerAni.SetTrigger("Jump");
            }
        }

        else if (!PlayerController.isGrounded)
        {
            Vertical_velocity -= Gravity * Time.deltaTime;
        }

// Reset Animations
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            Walking = false;
            Running = false;
        }

        #region Moving
        // Run If Fire3 Is Pushed And Moving
        if (Input.GetButton("Fire3"))
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                Running = false;
            }

            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Running = true;
            }

            Walking = false;
        }

// Else Walk If Moving
        else
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                Walking = false;
            }

            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Walking = true;
            }

            Running = false;
        }

// Double Movement Speed
        if (Running == true)
        {
            MoveLeftRight = Input.GetAxis("Horizontal") * MovementSpeed * 2;
            MoveFrontBack = Input.GetAxis("Vertical") * MovementSpeed * 2;
        }

// Normal Walking Speed
        else
        {
            MoveLeftRight = Input.GetAxis("Horizontal") * MovementSpeed;
            MoveFrontBack = Input.GetAxis("Vertical") * MovementSpeed;
        }

// Run & Walk Animations
        AnimationMovement();

        #endregion

// Movement Math
        Vector3 movement = new Vector3(MoveLeftRight, Vertical_velocity, MoveFrontBack);
        movement = character.rotation * movement;
        character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
        centerpoint.position = new Vector3(character.position.x, character.position.y + 4, character.position.z + 2);

// Camera Rotation
        if (Input.GetAxis("Vertical") != 0)
        {
            Quaternion TurnAngle = Quaternion.Euler(0, centerpoint.eulerAngles.y, 0);
            character.rotation = Quaternion.Slerp(character.rotation, TurnAngle, Time.deltaTime * RotationSpeed);
        }

       

    }

// Run Animations In Update
    void AnimationMovement()
    {
        if (Walking == true)
        {
            PlayerAni.SetBool("Walking", true);
            PlayerAni.SetBool("Running", false);
        }

        else if (Running == true)
        {
            PlayerAni.SetBool("Walking", false);
            PlayerAni.SetBool("Running", true);
        }

        else if (Walking == false && Running == false)
        {
            PlayerAni.SetBool("Walking", false);
            PlayerAni.SetBool("Running", false);
        }
    }
}
