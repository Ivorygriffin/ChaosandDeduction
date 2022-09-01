using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;



public class CustomNetworkManager : NetworkManager
{
    public UnityEvent<Alignment> onLeave = new UnityEvent<Alignment>();
    public UnityEvent<NetworkConnectionToClient> onJoin = new UnityEvent<NetworkConnectionToClient>();
    public UnityEvent onHost = new UnityEvent();

    public Dictionary<NetworkConnection, PlayerData> playersData = new Dictionary<NetworkConnection, PlayerData>();
    int connectedPlayers = 0;

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        CharacterInteraction interactor = null;
        foreach (NetworkIdentity networkIdentity in conn.clientOwnedObjects) //TODO: this does not work since player's object gets deleted too fast?
        {
            CharacterInteraction temp = networkIdentity.GetComponent<CharacterInteraction>();
            if (temp)
            {
                interactor = temp;
            }
        }
        //if (interactor)
        onLeave.Invoke(Alignment.Traitor);
        //player has disconnected
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        onJoin.Invoke(conn);
        //player has joined

        PlayerData temp = new PlayerData
        {
            alignment = Alignment.Traitor,
            modelIndex = connectedPlayers,
        };

        playersData.Add(conn, temp);
        //PlayerManager.Instance.TargetAssignRole(conn, temp);

        connectedPlayers++;

        if (connectedPlayers == 4)
            ChangeScene();
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
        onHost.Invoke();
        //started hosting
    }

    [Server]
    public void ChangeScene()
    {
        ServerChangeScene("SampleScene");
    }
}
