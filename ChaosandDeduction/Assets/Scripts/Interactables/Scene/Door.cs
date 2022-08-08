using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// An interactable door that opens/closes each time the player interacts with it
/// </summary>
public class Door : Interactable
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    [Header("Both Doors")]
    public float angle;
    public float transitionTime = 1.5f;

    [HideInInspector]
    public float timeTaken = 0;

    [HideInInspector]
    [SyncVar]
    public bool state = false;

    [Header("First Door")]
    public Vector3 doorAxis;
    public Transform door;


    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        doorAxis += transform.position;
    }


    protected void Update()
    {
        if (state && timeTaken < transitionTime)
        {
            timeTaken += Time.deltaTime;
            door.RotateAround(doorAxis, Vector3.up, (angle / transitionTime) * Time.deltaTime);
        }
        if (!state && timeTaken > 0)
        {
            timeTaken -= Time.deltaTime;
            door.RotateAround(doorAxis, Vector3.up, -(angle / transitionTime) * Time.deltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(doorAxis + transform.position, 0.1f);
    }
#endif


    //  Methods ---------------------------------------
    public override void InteractOverride(CharacterInteraction character)
    {
        CmdToggleState();
        //timeTaken = state ? 0 : transitionTime;
    }

    [Command(requiresAuthority = false)]
    void CmdToggleState()
    {
        state = !state;
    }

    public override void ResetInteractable()
    {
        base.ResetInteractable();

        timeTaken = 0;
        state = false;
    }


    //  Event Handlers --------------------------------
}
