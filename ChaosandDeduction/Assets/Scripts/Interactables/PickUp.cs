using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    bool pickedUp = false;
    Rigidbody rb;
    public float holdDistance = 1;

    //Grab points for objects

    public GrabPoint[] grabPoints;

    [System.Serializable]
    public class GrabPoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override bool Interact(CharacterInteraction character)
    {
        pickedUp = !pickedUp;
        if (pickedUp)
        {
            transform.parent = character.transform;
            transform.position = character.transform.position + (character.transform.forward * holdDistance); //place the object infront of the character
        }
        else
        {
            transform.parent = null;
        }

        if (rb != null)
            rb.isKinematic = pickedUp;

        return pickedUp;
    }
}
