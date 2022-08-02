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
        foreach (NetworkIdentity networkIdentity in conn.clientOwnedObjects)
        {
            CharacterInteraction temp = networkIdentity.GetComponent<CharacterInteraction>();
            if (temp)
            {
                interactor = temp;
            }
        }
        if (interactor)
            onLeave.Invoke(interactor.alignment);
        //player has disconnected
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
        onHost.Invoke();
        //started hosting
    }
}
