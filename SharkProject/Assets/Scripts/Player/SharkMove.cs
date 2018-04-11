using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkMove : MonoBehaviour {

    // Private Variables
    private CharacterController SharkController;
    private float MouseX, MouseY;
    private float MoveFrontBack; //MoveLeftRight;
    private Animator SharkAni;

    // Shark Stats
    private int Health = 100;
    private float MaxSpeed = 32;
    private float CurSpeed;

    // Dash - RampUp:1 = Ready. 2 = InProgress, 3 = Cooldown
    public int RampUp = 1;
    public int BoostType = 1;
    public float RampSpeed;
    public float RampTimer = 0;
    public bool Overdrive = false;

    // Public Variables
    public Transform playercam, character, centerpoint;
    public float MouseSensitivity = 10.0f;
    public float MovementSpeed = 2.0f;
    public float RotationSpeed = 25;
    public float jumpStrenght = 4;
    public float Vertical_velocity;
    public bool IsAlive = true;

    // Use this for initialization
    void Start ()
    {
        SharkController = GetComponent<CharacterController>();
        SharkAni = GetComponent<Animator>();
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsAlive)
        {
            Movement();
        }

        if (!IsAlive){}
    }

    void Movement()
    {
        // Camera Movement
        MouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        MouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        MouseY = Mathf.Clamp(MouseY, -30f, 48f);
        centerpoint.localRotation = Quaternion.Euler(MouseY, MouseX, 0);

        //Vertical_velocity = -Gravity * Time.deltaTime;


        if (Input.GetButton("Fire3") && RampUp == 1)
        {
            RampUp = 2;
            RampSpeed = 0;
            RampTimer = 0;

            if (CurSpeed < 20)
            {
                BoostType = 1;
            }

            else if (CurSpeed < 30 && CurSpeed >= 20)
            {
                BoostType = 2;
            }

            else if (CurSpeed >= 30)
            {
                BoostType = 3;
                Overdrive = true;
            }
        }

        if (RampUp == 2 && BoostType == 3 && RampSpeed < 20)
        {
            CurSpeed += 0.8f;
            RampSpeed += 0.8f;
        }

        else if (RampUp == 2 && BoostType == 2 && CurSpeed >= 10 && RampSpeed < 15)
        {
            CurSpeed += 0.6f;
            RampSpeed += 0.6f;
        }

        else if (RampUp == 2 && BoostType == 1 && RampSpeed < 15)
        {
            CurSpeed += 0.4f;
            RampSpeed += 0.4f;
        }


        //MoveLeftRight = Input.GetAxis("Horizontal") * MovementSpeed;
        if (Input.GetAxis("Vertical") > 0 && CurSpeed <= MaxSpeed || Overdrive == true)
        {
            if (Overdrive == false)
            {
                //MoveFrontBack = Input.GetAxis("Vertical") * MovementSpeed;
                if (CurSpeed < 16)
                {
                    CurSpeed += 0.02f;
                }

                else if (CurSpeed >= 16 || CurSpeed < 36)
                {
                    CurSpeed += 0.4f;
                }
            }
            else
            {
                CurSpeed += 0.3f;
                //RampSpeed += 0.3f;
            }
        }

        else if (Input.GetAxis("Vertical") == 0 && CurSpeed > 0 && CurSpeed <= 34)
        {
            if (Overdrive == false)
            {
                if (CurSpeed < 20)
                {
                    CurSpeed -= 0.08f;
                }

                else if (CurSpeed >= 20)
                {
                    CurSpeed -= 0.4f;
                }
            }

        }

        else if (CurSpeed >= 32 && RampUp == 3)
        {
            CurSpeed -= 0.6f;
        }

        // Resets
        if (CurSpeed < 2f)
        {
            CurSpeed = 2f;
        }

        if (Overdrive == true && RampSpeed >= 20)
        {
            RampUp = 3;
            RampSpeed = 0;
            Overdrive = false;

        }

        if (Overdrive == false && RampSpeed >= 15)
        {
            RampUp = 3;
            RampSpeed = 0;
        }

        if (RampUp == 3)
        {
            RampTimer += 0.15f;
            if (RampTimer >= 160)
            {
                RampUp = 1;
            }
        }

        // Set Animations & Movement
        SharkAni.SetFloat("SwimSpeed", CurSpeed);
        MoveFrontBack = CurSpeed * MovementSpeed;

        // Movement Math
        Vector3 movement = new Vector3(0, Vertical_velocity, MoveFrontBack);
        movement = character.rotation * movement;
        character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
        centerpoint.position = new Vector3(character.position.x, character.position.y + 4, character.position.z + 2);

        // Camera Rotation
        //if (Input.GetAxis("Vertical") != 0)
        //{
            Quaternion TurnAngle = Quaternion.Euler(centerpoint.eulerAngles.x, centerpoint.eulerAngles.y, 0);
            character.rotation = Quaternion.Slerp(character.rotation, TurnAngle, Time.deltaTime * RotationSpeed);
        //}
    }
}
