using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Allows objects to be picked up by the player
/// </summary>
public class Ingredient : PickUp
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



    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        startPos = transform.position;
    }

    //  Methods ---------------------------------------
    public override void Dropped(CharacterInteraction character)
    {
        transform.parent = null;
        transform.position = startPos;
    }

    //  Event Handlers --------------------------------
}