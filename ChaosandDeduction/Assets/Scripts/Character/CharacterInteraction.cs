using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//#if UNITY_EDITOR
using UnityEngine.InputSystem;
//#endif

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// The player's interaction component, allowing the player to interact with objects after giving input
/// </summary>
public class CharacterInteraction : NetworkBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    public float interactionRadius = 1;
    Interactable currentInteraction = null;


    //  Unity Methods ---------------------------------
    protected void Start()
    {
        if (isLocalPlayer)
            PlayerManager.Instance.localPlayer = gameObject;
    }


    protected void Update()
    {
        if (!isLocalPlayer)
            return;

        //#if UNITY_EDITOR
        if (Keyboard.current[Key.E].wasReleasedThisFrame)
        {
            Interact();
        }
        //#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube((transform.position + (transform.forward)), Vector3.one * 0.2f);
    }
#endif


    //  Methods ---------------------------------------
    public void Interact()
    {
        if (!currentInteraction)
        {
            Collider[] Results = Physics.OverlapSphere(transform.position + transform.forward, interactionRadius);

            if (Results.Length <= 0)
                return;

            float closestDistance = float.MaxValue;
            Interactable interactable = null;

            for (int i = 0; i < Results.Length; i++)
            {
                float distance = Vector3.Distance(transform.position + transform.forward, Results[i].transform.position); //calculate distance from grab point to object
                Interactable[] interactables = Results[i].GetComponents<Interactable>();

                for(int j = 0; j < interactables.Length; j++)
                if (Results[i] != null && interactables[j] != null && interactables[j].enabled && distance < closestDistance) //record the closest object
                {
                    closestDistance = distance;
                    interactable = interactables[j];
                }
            }


            // Interactable interactable = Results[closestIndex].GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactable.Interact(this)) //if is only used for checking if the interaction should be kept, it will be interacted with either way here
                    currentInteraction = interactable;
            }

        }
        else
        {
            if (!currentInteraction.Interact(this)) // check if we can forget about the object
                currentInteraction = null;
        }
    }


    //  Event Handlers --------------------------------
}
