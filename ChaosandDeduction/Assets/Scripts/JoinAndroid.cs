using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JoinAndroid : MonoBehaviour
{
    private void Start()
    {
        //NetworkManager.singleton.on
    }
    public void ButtonDown()
    {
        NetworkManager.singleton.networkAddress = "192.168.126.131";
        NetworkManager.singleton.StartClient();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
