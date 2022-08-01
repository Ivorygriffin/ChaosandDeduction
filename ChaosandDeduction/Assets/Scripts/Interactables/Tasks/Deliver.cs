using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Allows player to pickup object and attempt to bring it to the destination
/// </summary>
public class Deliver : PickUp
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
    Vector3 startPos;
    public Vector3 deliverPoint = Vector3.zero;
    public float deliverRadius = 5;


    public bool useResetTimer = false;
    public float resetMaxTimer = 5;
    float resetTimer = 0;



    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        startPos = transform.position;
    }


    protected void Update()
    {
        if (useResetTimer && resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer < 0)
                transform.position = startPos;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(deliverPoint, deliverRadius);
    }
#endif

    //  Methods ---------------------------------------
    public override void PickedUp(Transform character) //These will be RPCed from base pickup's interact
    {
        base.PickedUp(character);

        resetTimer = -1;
    }

    public override void Dropped(Transform character) //These will be RPCed from base pickup's interact
    {
        base.Dropped(character);

        if (Vector3.Distance(transform.position, deliverPoint) < deliverRadius)
            NetworkServer.Destroy(gameObject); //TODO: create ingredient class that ondestroy adds point
                                               //Destroy(gameObject); 

        resetTimer = resetMaxTimer;
    }


    //  Event Handlers --------------------------------
}