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
    public float animationMultiplier = 0.8f;

    Vector3 playerVelocity;
    Vector3 lastFramePos = Vector3.zero;
    public Animator animator;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        character = GetComponent<CharacterController>();
        joystick = Joystick.Instance;
    }


    protected void Update()
    {
        if (!isLocalPlayer)
        {
            if (animator) //TODO: determine if this is most efficent way to do this? currently if not calculating speed we just get the distance per frame
            {
                animator.SetFloat("Blend", Vector3.Distance(lastFramePos, transform.position) / (Time.deltaTime));
                lastFramePos = transform.position;
            }
            else
                animator = transform.GetChild(GetComponent<CharacterInteraction>().modelIndex).gameObject.GetComponent<Animator>();

            return;
        }

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
        if (animator)
            animator.SetFloat("Blend", playerVelocity.magnitude);

        //groundedPlayer = character.isGrounded;
    }


    //  Methods ---------------------------------------



    //  Event Handlers --------------------------------
}
