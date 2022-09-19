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
    public Vector3 deliverPoint = Vector3.zero;
    public float deliverRadius = 5;


    public bool useResetTimer = false;
    public float resetMaxTimer = 5;
    float resetTimer = 0;

    public bool destroyOnArrival = true;
    public bool disabledTimer = false;

    [SerializeField] Reward reward;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();

        //if (reward.interactable)
        //{
        //    reward.interactable.enabled = false;
        //    reward.interactable.Start();
        //}
    }


    protected void Update()
    {
        if (useResetTimer && !disabledTimer && resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer < 0)
                transform.position = startPos;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (Selection.activeGameObject == gameObject)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(deliverPoint, deliverRadius);
        }

        if (reward != null)
            reward.drawGizmo(transform);
    }
#endif

    //  Methods ---------------------------------------
    public override void PickedUp(Transform character) //These will be RPCed from base pickup class's interact
    {
        base.PickedUp(character);

        resetTimer = -1;
    }

    public override void Dropped(Transform character) //These will be RPCed from base pickup class's interact
    {
        base.Dropped(character);

        if (Vector3.Distance(transform.position, deliverPoint) < deliverRadius)
        {
            //StartCoroutine(DelayDestroy());
            //Destroy(gameObject); 
            reward.LocalReward();
            useable = false;
            CmdReward();
        }

        resetTimer = resetMaxTimer;
    }


    [Command(requiresAuthority = false)]
    void CmdReward()
    {
        reward.ServerReward(transform);

        if (destroyOnArrival)
            NetworkServer.Destroy(gameObject);
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(5);
    }
    public override void ResetInteractable()
    {
        base.ResetInteractable();

        resetTimer = 0;
        reward.Reset();
    }

    //  Event Handlers --------------------------------
}