using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCharacter : NetworkBehaviour
{
    CustomNetworkManager netManager;
    public GameObject[] models;

    private void Start()
    {
        if (isServer)
        {
            netManager = (CustomNetworkManager)NetworkManager.singleton;
            //netManager.onJoin.AddListener(CmdUpdateModel); //onJoin is too slow for the client that is connecting
            netManager.onLeave.AddListener(CmdUpdateModel);
        }
        CmdUpdateModel();
    }

    private void OnDestroy()
    {
        if (isServer && netManager != null)
        {
            //netManager.onJoin.RemoveListener(CmdUpdateModel);
            netManager.onLeave.RemoveListener(CmdUpdateModel);
        }
    }

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    CmdUpdateModel();
    //}
    //public override void OnStopClient()
    //{
    //    base.OnStopClient();
    //    CmdUpdateModel();
    //}

    [Command(requiresAuthority = false)]
    public void CmdUpdateModel()
    {
        //pass all the data to player (the network manager keeps all playerdata on the host only)

        short[] data = new short[netManager.playerArray.Length]; //using short rather than byte to support -1 as undefined
        for (int i = 0; i < data.Length; i++)
        {
            if (netManager.playerArray[i].alignment == Alignment.Undefined)
                data[i] = -1;
            else
                data[i] = netManager.playerArray[i].modelIndex;
        }

        RpcUpdateModel(data);
    }

    [ClientRpc]
    void RpcUpdateModel(short[] modelIndices)
    {
        bool[] activeModels = new bool[models.Length]; //default all models inactive


        for (int i = 0; i < modelIndices.Length; i++) //find all players that should be active
        {
            if (modelIndices[i] != -1) //test if player is undefined
                activeModels[modelIndices[i]] = true;
        }

        for (int i = 0; i < models.Length; i++)
            models[i].SetActive(activeModels[i]);
    }
}
