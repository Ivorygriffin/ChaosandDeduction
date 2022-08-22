using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class JoinAndroid : MonoBehaviour
{
    public NetworkDiscoveryHUD networkDiscovery;
    private void Start()
    {
        //NetworkManager.singleton.on
#if UNITY_EDITOR
        NetworkManager.singleton.networkAddress = "localhost";
#endif
    }
    public void ButtonDown()
    {
        //NetworkManager.singleton.StartClient();
        networkDiscovery.Discover();

        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
