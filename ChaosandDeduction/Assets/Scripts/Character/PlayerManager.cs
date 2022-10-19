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
            characterMovement = player.GetComponent<CharacterMovement>();
            characterInteraction = player.GetComponent<CharacterInteraction>();
            characterFire = player.GetComponent<CharacterFire>();
            //localAlignment = Alignment.Villager;
            //CmdAssignRole(value);
        }
    }
    public CharacterMovement characterMovement { get; private set; }
    public CharacterInteraction characterInteraction { get; private set; }
    public CharacterFire characterFire { get; private set; }


    public PlayerData localPlayerData;

    //  Fields ----------------------------------------
    public static PlayerManager Instance;

    [SyncVar]
    bool traitorAssigned = false;
    [SyncVar]
    public int playersJoined = 0;

    public Vector3[] spawnPoints;

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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            switch (i)
            {
                case 0:
                    Gizmos.color = Color.red;
                    break;
                case 1:
                    Gizmos.color = Color.magenta;
                    break;
                case 2:
                    Gizmos.color = Color.blue;
                    break;
                case 3:
                    Gizmos.color = Color.yellow;
                    break;
                default:
                    Gizmos.color = Color.white;
                    break;
            }
            Gizmos.DrawSphere(spawnPoints[i], 0.2f);
        }
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
