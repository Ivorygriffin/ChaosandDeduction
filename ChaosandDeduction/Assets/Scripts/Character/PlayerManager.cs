using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//  Namespace Properties ------------------------------
[System.Serializable]
public struct PlayerData
{
    public Alignment alignment;
    public byte modelIndex;
}

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
            //localAlignment = Alignment.Villager;
            //CmdAssignRole(value);
        }
    }
    public PlayerData localPlayerData;

    //  Fields ----------------------------------------
    public static PlayerManager Instance;

    [SyncVar]
    bool traitorAssigned = false;
    [SyncVar]
    public int playersJoined = 0;

    //public GameObject[] allPlayers = new GameObject[4];

    GameObject player;


    //  Unity Methods ---------------------------------
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


        CmdGetData();
        //localPlayerData = CustomNetworkManager.singleton;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }


    //  Methods ---------------------------------------
    [Command(requiresAuthority = false)]
    void CmdGetData(NetworkConnectionToClient connection = null)
    {
        CustomNetworkManager temp = (CustomNetworkManager)CustomNetworkManager.singleton;

        int index = -1;
        temp.playersIndex.TryGetValue(connection, out index);

        PlayerData data;
        data = temp.playerArray[index];

        //connectionToClient;
        TargetAssignData(connection, data);
    }
    [TargetRpc]
    void TargetAssignData(NetworkConnection target, PlayerData data)
    {
        //player.alignment = Alignment.Traitor;
        localPlayerData = data;
    }

    [Server]
    public void playerLeft(Alignment alignment)
    {
        playersJoined--;
        if (alignment == Alignment.Traitor)
            traitorAssigned = false;
    }

    //  Event Handlers --------------------------------
}
