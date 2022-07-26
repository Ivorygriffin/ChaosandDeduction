
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using ParrelSync;
#endif
using Mirror;
using Mirror.Discovery;

public class Autojoin : MonoBehaviour
{
    public CustomNetworkManager networkManager;
    public NetworkDiscoveryHUD discovery;

    private void Start()
    {
        //TODO: not used in just editor, must replace this at some point?
        // cant be assigned early / serialized, due to dont destroy on load?
        networkManager = (CustomNetworkManager)NetworkManager.singleton;
        discovery = networkManager.gameObject.GetComponent<NetworkDiscoveryHUD>();
    }

#if UNITY_EDITOR
    public bool doAuto = true;
    public bool joining = false;
    private void Update()
    {
        if (!LoadingScreenManager.instance || joining || !doAuto)
            return;

        if (ClonesManager.IsClone())
        {
            discovery.Join();
            // Automatically connect to local host if this is the clone editor
        }
        else
        {
            discovery.Host();
            // Automatically start server if this is the original editor
        }
        joining = true;
    }
#endif

    public void Join()
    {
        discovery.Join();
    }
    public void Host()
    {
        discovery.Host();
    }


    //[UnityEditor.Callbacks.DidReloadScripts]
    //private static void OnScriptsReloaded()
    //{
    //    // do something
    //    EditorApplication.ExitPlaymode();
    //}
}
