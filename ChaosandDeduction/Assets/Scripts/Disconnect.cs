using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Disconnect : MonoBehaviour
{
    public void OnClick()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
    }
}
