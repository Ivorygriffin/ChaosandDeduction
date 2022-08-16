using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JoinAndroid : MonoBehaviour
{
    private void Start()
    {
        //NetworkManager.singleton.on
#if UNITY_EDITOR
        NetworkManager.singleton.networkAddress = "localhost";
#endif
    }
    public void ButtonDown()
    {
        NetworkManager.singleton.StartClient();

        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
