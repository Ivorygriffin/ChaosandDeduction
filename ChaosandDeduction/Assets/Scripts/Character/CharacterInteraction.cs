using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

//  Namespace Properties ------------------------------
public enum Alignment
{
    Neutral,
    Villager,
    Traitor
}

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
    public Interactable currentInteraction = null;
    public Alignment alignment = Alignment.Villager; //TOOD: distribute roles
    [SyncVar(hook = "ChangeModel")]
    public int modelIndex = 0;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        transform.GetChild(modelIndex).gameObject.SetActive(true);
        //if (isLocalPlayer)
        //    PlayerManager.Instance.localPlayer = gameObject;
        if (isServer) //server does not gain the effects of changing model, thus assume the first model is theirs
        {
            Animator animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
            //animator.speed = 0.5f;
            GetComponent<CharacterMovement>().animator = animator;
        }
    }


    protected void Update()
    {
        if (!isLocalPlayer)
            return;
        else if (PlayerManager.Instance && !PlayerManager.Instance.localPlayer)
            PlayerManager.Instance.localPlayer = gameObject;

#if UNITY_EDITOR
        if (Keyboard.current[Key.E].wasReleasedThisFrame)
        {
            Interact();
        }
#endif
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

                for (int j = 0; j < interactables.Length; j++)
                    if (Results[i] != null && interactables[j] != null && interactables[j].enabled && distance < closestDistance) //record the closest object
                    {
                        closestDistance = distance;
                        interactable = interactables[j];
                    }
            }


            // Interactable interactable = Results[closestIndex].GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }

        }
        else
        {
            currentInteraction.Interact(this);
        }
    }
    protected void ChangeModel(int oldVar, int newVar)
    {
        transform.GetChild(oldVar).gameObject.SetActive(false);
        GameObject newModel = transform.GetChild(newVar).gameObject;
        newModel.SetActive(true);

        Animator animator = transform.GetChild(newVar).gameObject.GetComponent<Animator>();
        //animator.speed = 0.5f;
        GetComponent<CharacterMovement>().animator = animator;
    }

    //  Event Handlers --------------------------------
}
