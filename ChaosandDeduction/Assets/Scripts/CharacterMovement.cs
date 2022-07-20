using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Joystick joystick;
    CharacterController character;

    public float moveSpeed = 5;

    Vector3 playerVelocity;
    bool groundedPlayer;
    float gravityValue = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = Vector3.zero;

        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        playerVelocity += new Vector3(joystick.inputVector.x, 0, joystick.inputVector.y) * moveSpeed;
        //playerVelocity.y += gravityValue * Time.deltaTime;

        character.SimpleMove(playerVelocity ); //* Time.deltaTime

        //groundedPlayer = character.isGrounded;
    }
}
