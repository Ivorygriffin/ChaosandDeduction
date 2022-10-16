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
    Undefined,
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
    //public Alignment alignment = Alignment.Villager; //TOOD: distribute roles
    [SyncVar]
    public int modelIndex = -1;

    public AudioSource AudioSource;
    public AudioClip NoInteractable;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        //transform.GetChild(modelIndex).gameObject.SetActive(true);
        //if (isLocalPlayer)
        //    PlayerManager.Instance.localPlayer = gameObject;
        if (isServer) //server does not gain the effects of changing "ChangeModel" being called, thus assume the first model is theirs
        {
            CmdChangeModel(0); //if server host, assume index 0
        }
        else if (modelIndex != -1)
            ChangeModel(modelIndex);
    }


    protected void Update()
    {
        if (!isLocalPlayer)
            return;
        else if (PlayerManager.Instance) //TODO: move this to a custom player spawn message
        {
            if (!PlayerManager.Instance.localPlayer) //late assign local player as this object
                PlayerManager.Instance.localPlayer = gameObject;

            //Check if modelIndex wasnt assigned yet and localPlayerData has been assigned
            if (modelIndex != PlayerManager.Instance.localPlayerData.modelIndex)
            {
                CmdChangeModel(PlayerManager.Instance.localPlayerData.modelIndex);
                //alignment = PlayerManager.Instance.localPlayerData.alignment;
            }
        }
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
            Interactable[] interactable = null;

            for (int i = 0; i < Results.Length; i++)
            {
                float distance = Vector3.Distance(transform.position + transform.forward, Results[i].transform.position); //calculate distance from grab point to object
                Interactable[] interactables = Results[i].GetComponents<Interactable>();

                //for (int j = 0; j < interactables.Length; j++)
                if (Results[i] != null && interactables != null && interactables.Length > 0 && distance < closestDistance) //record the closest object
                {
                    closestDistance = distance;
                    interactable = interactables;
                }
            }


            // Interactable interactable = Results[closestIndex].GetComponent<Interactable>();

            if (interactable != null)
                for (int i = 0; i < interactable.Length; i++)
                    if (interactable[i] != null)
                        interactable[i].Interact(this);

        }
        else
        {
            currentInteraction.Interact(this);
        }
    }
    [Command(requiresAuthority = false)]
    void CmdChangeModel(int index)
    {
        modelIndex = index;
        RpcChangeModel(index);
    }

    [ClientRpc]
    protected void RpcChangeModel(int index)
    {
        ChangeModel(index);
    }

    void ChangeModel(int index)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject newModel = transform.GetChild(index).gameObject;
        newModel.SetActive(true);

        //animator.speed = 0.5f;
        CharacterMovement mover = GetComponent<CharacterMovement>();

        if (PlayerManager.Instance) //TODO: move this? somewhere?
            mover.Teleport(PlayerManager.Instance.spawnPoints[index]);

        //mover.animator.animator = newModel.GetComponent<Animator>();
        mover.animator = newModel.GetComponent<Animator>();
        mover.footsteps = newModel.GetComponent<AudioPlayer>();
    }

    //  Event Handlers --------------------------------
}
