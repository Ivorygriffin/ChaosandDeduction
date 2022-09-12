
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
    private void Start()
    {
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
    }
#endif
    //[UnityEditor.Callbacks.DidReloadScripts]
    //private static void OnScriptsReloaded()
    //{
    //    // do something
    //    EditorApplication.ExitPlaymode();
    //}
}
