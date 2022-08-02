using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public UnityEvent<Alignment> onLeave = new UnityEvent<Alignment>();
    public UnityEvent onHost = new UnityEvent();

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
    public override void OnStartHost()
    {
        base.OnStartHost();
        onHost.Invoke();
        //started hosting
    }
}
