using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// A networked manager that is used to assign player variables between scenes and allocate a traitor, along with being a easy way to get the local player
/// </summary>
public class PlayerManager : NetworkBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public GameObject localPlayer
    {
        get { return player; }
        set
        {
            player = value;
            CmdAssignRole(value);
        }
    }


    //  Fields ----------------------------------------
    public static PlayerManager Instance;

    [SyncVar]
    bool traitorAssigned = false;
    [SyncVar]
    int playersJoined = 0;
    GameObject player;


    //  Unity Methods ---------------------------------
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    //  Methods ---------------------------------------
    [Command(requiresAuthority = false)]
    void CmdAssignRole(GameObject player)
    {
        playersJoined++;
        if (!traitorAssigned) //if no traitor yet, and 
        {
            if (Random.Range(0, 4) == 0 || playersJoined >= 4)
            {
                CharacterInteraction interactor  = player.GetComponent<CharacterInteraction>();
                NetworkIdentity target = player.GetComponent<NetworkIdentity>();

                TargetAssignTraitor(target.connectionToClient, interactor);

                interactor.alignment = Alignment.Traitor;
            }
        }
    }
    [TargetRpc]
    void TargetAssignTraitor(NetworkConnection target, CharacterInteraction player)
    {
        player.alignment = Alignment.Traitor;
    }

    //  Event Handlers --------------------------------
}
