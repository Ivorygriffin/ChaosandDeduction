using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Double door variation of the door interactable
/// </summary>
public class DoubleDoor : Door
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    [Header("Second Door")]
    public Vector3 doorAxis2;
    public Transform door2;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        doorAxis += transform.position;
        doorAxis2 += transform.position;
    }


    protected void Update()
    {
        if (state && timeTaken < transitionTime)
        {
            timeTaken += Time.deltaTime;
            door.RotateAround(doorAxis, Vector3.up, (angle / transitionTime) * Time.deltaTime);
            door2.RotateAround(doorAxis2, Vector3.up, -(angle / transitionTime) * Time.deltaTime);
        }
        if (!state && timeTaken > 0)
        {
            timeTaken -= Time.deltaTime;
            door.RotateAround(doorAxis, Vector3.up, -(angle / transitionTime) * Time.deltaTime);
            door2.RotateAround(doorAxis2, Vector3.up, (angle / transitionTime) * Time.deltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(doorAxis + transform.position, 0.1f);
        Gizmos.DrawSphere(doorAxis2 + transform.position, 0.1f);
    }
#endif

    //  Methods ---------------------------------------
  


    //  Event Handlers --------------------------------
}
