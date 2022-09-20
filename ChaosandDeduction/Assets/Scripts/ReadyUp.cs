using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReadyUp : NetworkBehaviour
{
    public bool[] readyState = new bool[4];
    CustomNetworkManager networkManager;
   public bool state = false;

    public GameObject readyButton;
    public GameObject unreadyButton;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = (CustomNetworkManager)NetworkManager.singleton;

        readyButton.SetActive(!state);
        unreadyButton.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ready()
    {
        CmdReady(!state);
    }

    [Command(requiresAuthority = false)]
    void CmdReady(bool ready, NetworkConnectionToClient conn = null)
    {
        readyState[networkManager.playersIndex[conn]] = ready;

        TargetReady(conn, ready);

        //change to real game when ready
        bool start = true;
        for (int i = 0; i < 1; i++)
            if (!readyState[i])
                start = false;
        if (start)
            networkManager.ChangeScene();
    }
    [TargetRpc]
    void TargetReady(NetworkConnection conn, bool ready)
    {
        state = ready;

        readyButton.SetActive(!state);
        unreadyButton.SetActive(state);
    }
}
