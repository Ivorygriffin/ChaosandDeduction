using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
    [SyncVar]
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
    public override bool InteractOverride(CharacterInteraction character)
    {
        pickedUp = !pickedUp;
        if (pickedUp)
        {
            CmdPickedUp(character.transform);
        }
        else
        {
            CmdDropped(character.transform);
        }

        return pickedUp;
    }



    [Command(requiresAuthority = false)]
    void CmdPickedUp(Transform character)
    {
        RpcPickedUp(character);
    }
    [ClientRpc]
    void RpcPickedUp(Transform character)
    {
        PickedUp(character);
    }
    public virtual void PickedUp(Transform character)
    {
        float closestPoint = float.MaxValue;
        int closestIndex = 0;
        for (int i = 0; i < grabPoints.Length; i++)
        {
            float distance = Vector3.Distance(grabPoints[i].position, character.position);
            if (distance < closestPoint)
            {
                closestPoint = distance;
                closestIndex = i;
            }
        }
        transform.parent = character;

        Vector3 EulerShenanigans = grabPoints[closestIndex].localRotation.eulerAngles; //TODO: WHY THE FUCK DOES THIS FIX IT? (inverse the y axis (pulling the Z through itself))
        EulerShenanigans.y -= 90;
        EulerShenanigans *= -1;
        EulerShenanigans.y += 90;

        transform.localRotation = Quaternion.Euler(EulerShenanigans); //grabPoints[closestIndex].localRotation * character.transform.rotation;// * transform.rotation;
        transform.position = (character.position + (character.forward * holdDistance)) + (transform.position - grabPoints[closestIndex].position); //place the object infront of the character
#if UNITY_EDITOR
        secondLastGrabbed = lastGrabbed;
        lastGrabbed = closestIndex;
#endif

        if (rb != null)
            rb.isKinematic = true;
    }



    [Command(requiresAuthority = false)]
    void CmdDropped(Transform character)
    {
        RpcDropped(character);
    }
    [ClientRpc]
    void RpcDropped(Transform character)
    {
        Dropped(character);
    }
    public virtual void Dropped(Transform character)
    {
        transform.parent = null;

        if (rb != null)
            rb.isKinematic = false;
    }

    //  Event Handlers --------------------------------
}