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
    public CharacterController characterController;

    float moveSpeed = 5;
    public const float fullMoveSpeed = 5;
    public float glueMoveSpeed = 2.5f;
    public float animationMultiplier = 0.8f;

    Vector3 playerVelocity;
    //public NetworkAnimator animator;
    public Character character;

    public bool thirdPerson = true;
    float lookSpeed = 120;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        //character = GetComponent<CharacterController>();
    }


    protected void Update()
    {
        if (!isLocalPlayer)
        {
            if (character) //TODO: determine if this is most efficent way to do this? currently if not calculating speed we just get the distance per frame
                character.RemoteUpdate();
            //  else
            //      animator = transform.GetChild(GetComponent<CharacterInteraction>().modelIndex).gameObject.GetComponent<Animator>();

            return;
        }

        playerVelocity = Vector3.zero;

        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}
        Vector3 inputVector = Vector3.zero;

        if (Joystick.Instance)
        {
            inputVector = new Vector3(Joystick.Instance.inputVector.x, 0, Joystick.Instance.inputVector.y);
            if (thirdPerson)
                playerVelocity += inputVector * moveSpeed;
            else
                playerVelocity = inputVector.z * transform.forward * moveSpeed;
        }
        //playerVelocity.y += gravityValue * Time.deltaTime;

        // playerVelocity = transform.TransformDirection(playerVelocity);

        if (inputVector != Vector3.zero)
        {
            if (thirdPerson)
                gameObject.transform.forward = playerVelocity;
            else
                transform.Rotate(0, inputVector.x * Time.deltaTime * lookSpeed, 0);

        }


        characterController.SimpleMove(playerVelocity); //* Time.deltaTime
        if (character) //TODO: determine if this is most efficent way to do this? currently if not calculating speed we just get the distance per frame
            character.LocalUpdate(playerVelocity);

        //groundedPlayer = character.isGrounded;
    }

    public void Teleport(Vector3 point)
    {
        characterController.enabled = false;
        transform.position = point;
        characterController.enabled = true;
    }

    //  Methods ---------------------------------------

    public void TriggerGlue(bool slow)
    {
        moveSpeed = slow ? glueMoveSpeed : fullMoveSpeed;
    }

    //  Event Handlers --------------------------------
}
