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
    //[SyncVar]
    public bool pickedUp = false;
    float holdDistance = 1;
    Rigidbody rb;

    [SyncVar]
    CharacterInteraction currentOwner;

    //Grab points for objects
    [Header("Empty game objects")]
    public Transform[] grabPoints;
#if UNITY_EDITOR
    int lastGrabbed = -1;
    int secondLastGrabbed = -1;
#endif
    [Header("Pickup Sounds")]
    public AudioClip pickupSound;
    public AudioClip dropSound;




    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }


    protected void Update()
    {

    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
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
    protected override void InteractOverride(CharacterInteraction character)
    {
        UIManager.Instance.ResetWandProgress();

        if (!pickedUp)
        {
            CmdPickedUp(character);
            character.currentInteraction = this;

            audioSource.PlayOneShot(pickupSound);
        }
        else //if(character == currentOwner)
        {
            CmdDropped(character);
            character.currentInteraction = null;

            audioSource.PlayOneShot(dropSound);

        }
    }



    [Command(requiresAuthority = false)]
    void CmdPickedUp(CharacterInteraction character)
    {
        RpcPickedUp(character.transform);
        currentOwner = character;
    }
    [ClientRpc]
    void RpcPickedUp(Transform character)
    {
        PickedUp(character);


    }
    public virtual void PickedUp(Transform character)
    {
        pickedUp = true;


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
    void CmdDropped(CharacterInteraction character)
    {
        RpcDropped(character.transform);
        //currentOwner = null; //TODO: determine if not setting null will upset anywhere, can not implement here due to delay between RPC and setting null
    }
    [ClientRpc]
    void RpcDropped(Transform character)
    {
        Dropped(character);
        currentOwner.currentInteraction = null; //double jeapody or something (just ensuring a second player wasnt the cause of dropping the item)

    }
    public virtual void Dropped(Transform character)
    {
        pickedUp = false;

        transform.parent = null;

        if (rb != null)
            rb.isKinematic = false;
    }


    //  Event Handlers --------------------------------
}