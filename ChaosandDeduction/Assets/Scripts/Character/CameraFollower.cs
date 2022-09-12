using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// A simple script that follows a target at a fixed angle and distance
/// </summary>
public class CameraFollower : MonoBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------


    //  Fields ----------------------------------------
    public Transform target;
    public Vector3 offset;


    //  Unity Methods ---------------------------------
    protected void Start()
    {

    }


    protected void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
        else
            if (PlayerManager.Instance && PlayerManager.Instance.localPlayer)
            target = PlayerManager.Instance.localPlayer.transform;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(target.position + offset, Vector3.one * 0.25f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(target.position + offset, transform.forward * 3);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.DrawFrustum(target.position + offset, 60, 1000, 0.3f, 16.0f / 9.0f);
        }
    }
#endif


    //  Methods ---------------------------------------


    //  Event Handlers --------------------------------

}
