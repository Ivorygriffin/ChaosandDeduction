using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Allows player to pickup object and attempt to bring it to the destination
/// </summary>
public class DeliverMorph : Deliver
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    //[System.Serializable]
    //public class GrabPoint
    //{
    //    public Vector3 Position;
    //    public Quaternion Rotation;
    //}


    //  Fields ----------------------------------------
    public GameObject[] stages;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        stages[0].SetActive(true);
        stages[1].SetActive(false);
    }

    //  Methods ---------------------------------------
    public override void Dropped(Transform character) //These will be RPCed from base pickup class's interact
    {
        if (Vector3.Distance(transform.position, deliverPoint) < deliverRadius)
        {
            stages[0].SetActive(false);
            stages[1].SetActive(true);
        }
        base.Dropped(character);
    }

    //  Event Handlers --------------------------------
}