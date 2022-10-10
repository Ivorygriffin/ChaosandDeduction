
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
    public NetworkDiscoveryHUD discovery;
#if UNITY_EDITOR
    public bool joining = false;
    private void Update()
    {
        if (!LoadingScreenManager.instance || joining)
            return;

        if (discovery == null) //just for editor so performance doesnt matter too much
            discovery = FindObjectOfType<NetworkDiscoveryHUD>();


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
        LoadingScreenManager.instance.LoadScreen();
    }
#endif
    //[UnityEditor.Callbacks.DidReloadScripts]
    //private static void OnScriptsReloaded()
    //{
    //    // do something
    //    EditorApplication.ExitPlaymode();
    //}
}
