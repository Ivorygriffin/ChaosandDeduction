using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Allows objects to be picked up by the player
/// </summary>
public class PickUp : Interactable
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    //[System.Serializable]
    //public class GrabPoint
    //{
    //    public Vector3 Position;
    //    public Quaternion Rotation;
    //}


    //  Fields ----------------------------------------
    bool pickedUp = false;
    Rigidbody rb;
    float holdDistance = 1;

    //Grab points for objects
    [Header("Empty game objects")]
    public Transform[] grabPoints;
#if UNITY_EDITOR
    int lastGrabbed = -1;
    int secondLastGrabbed = -1;
#endif


    //  Unity Methods ---------------------------------
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected void Update()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < grabPoints.Length; i++)
        {
            if (i == lastGrabbed)
                Gizmos.color = Color.red;
            else if (i == secondLastGrabbed)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.grey;

            Gizmos.DrawSphere(grabPoints[i].position, 0.1f);
            Gizmos.DrawLine(grabPoints[i].position, grabPoints[i].position + (grabPoints[i].rotation * Vector3.forward * 0.5f));

        }
    }
#endif

    //  Methods ---------------------------------------
    public override bool Interact(CharacterInteraction character)
    {
        pickedUp = !pickedUp;
        if (pickedUp)
        {
            PickedUp(character);
        }
        else
        {
            Dropped(character);
        }

        if (rb != null)
            rb.isKinematic = pickedUp;

        return pickedUp;
    }

    public virtual void PickedUp(CharacterInteraction character)
    {
        float closestPoint = float.MaxValue;
        int closestIndex = 0;
        for (int i = 0; i < grabPoints.Length; i++)
        {
            float distance = Vector3.Distance(grabPoints[i].position, character.transform.position);
            if (distance < closestPoint)
            {
                closestPoint = distance;
                closestIndex = i;
            }
        }
        transform.parent = character.transform;

        Vector3 EulerShenanigans = grabPoints[closestIndex].localRotation.eulerAngles; //TODO: WHY THE FUCK DOES THIS FIX IT? (inverse the y axis (pulling the Z through itself))
        EulerShenanigans.y -= 90;
        EulerShenanigans *= -1;
        EulerShenanigans.y += 90;

        transform.localRotation = Quaternion.Euler(EulerShenanigans); //grabPoints[closestIndex].localRotation * character.transform.rotation;// * transform.rotation;
        transform.position = (character.transform.position + (character.transform.forward * holdDistance)) + (transform.position - grabPoints[closestIndex].position); //place the object infront of the character
#if UNITY_EDITOR
        secondLastGrabbed = lastGrabbed;
        lastGrabbed = closestIndex;
#endif
    }

    public virtual void Dropped(CharacterInteraction character)
    {
        transform.parent = null;
    }

    //  Event Handlers --------------------------------
}