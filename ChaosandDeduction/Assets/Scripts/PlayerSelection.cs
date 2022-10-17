using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSelection : NetworkBehaviour
{
    public LobbyCharacter lobbyCharacter;


    public void SetModel(int index)
    {
        CmdSetCharacter((byte)index);
    }

    [Command(requiresAuthority = false)]
    void CmdSetCharacter(byte index, NetworkConnectionToClient conn = null)
    {
        CustomNetworkManager temp = (CustomNetworkManager)CustomNetworkManager.singleton;

        temp.playerArray[temp.playersIndex[conn]].modelIndex = index;

        lobbyCharacter.CmdUpdateModel();
    }
}
