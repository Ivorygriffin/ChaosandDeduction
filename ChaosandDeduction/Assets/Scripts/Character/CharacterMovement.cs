using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//  Namespace Properties ------------------------------

//  Class Attributes ----------------------------------

/// <summary>
/// This script handles all the movement for the player, recieving input from a virtual joystick
/// </summary>

public class CharacterMovement : NetworkBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    public Joystick joystick;
    CharacterController character;

    public float moveSpeed = 5;

    Vector3 playerVelocity;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        character = GetComponent<CharacterController>();
        joystick = Joystick.Instance;
    }


    protected void Update()
    {
        if (!isLocalPlayer)
            return;

        playerVelocity = Vector3.zero;

        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        playerVelocity += new Vector3(joystick.inputVector.x, 0, joystick.inputVector.y) * moveSpeed;
        //playerVelocity.y += gravityValue * Time.deltaTime;
        if (playerVelocity != Vector3.zero)
            gameObject.transform.forward = playerVelocity;

        character.SimpleMove(playerVelocity); //* Time.deltaTime

        //groundedPlayer = character.isGrounded;
    }


    //  Methods ---------------------------------------



    //  Event Handlers --------------------------------
}
