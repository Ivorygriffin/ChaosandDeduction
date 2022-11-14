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
    public AudioClip deliverSound;

    [Header("Delivery settings")]
    public Vector3 deliverPoint = Vector3.zero;
    public float deliverRadius = 5;

    public TaskScriptableObject heldTask;
    public int heldTaskStage;

    [Header("Respawn")]
    public bool useResetTimer = false;
    public float resetMaxTimer = 5;
    float resetTimer = 0;

    public bool destroyOnArrival = true;
    public bool disabledTimer = false;

    public float cooldown = 0;

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
        if (useResetTimer && !disabledTimer && !pickedUp && resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
                ResetInteractable();
        }
        else if (Vector3.Distance(transform.position, startPos) > 3 && !disabledTimer && !pickedUp && resetTimer <= 0)
            resetTimer = resetMaxTimer;
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

    private void OnValidate()
    {
        base.OnValidate();

        if (heldTask != null && gameObject.scene.name != null) //may not get assigned if not yet spawned?
            heldTask.paths[heldTaskStage].endPosition = transform.position;

        if (reward.task != null)
        {
            reward.task.paths[reward.taskStage].endPosition = deliverPoint;

            if (reward.item != null && reward.taskStage + 1 < reward.task.paths.Length)
                reward.task.paths[reward.taskStage + 1].startPosition = reward.spawnPoint;

            if (gameObject.scene.name != null) //may not get assigned if not yet spawned?
                reward.task.paths[reward.taskStage].startPosition = transform.position;
        }
    }
#endif

    //  Methods ---------------------------------------
    protected override void InteractOverride(CharacterInteraction character)
    {
        //NavManager.instance.target = deliverPoint;
        base.InteractOverride(character);
    }

    public override void PickedUp(Transform character) //These will be RPCed from base pickup class's interact
    {
        base.PickedUp(character);

        if (heldTask)
            heldTask.isComplete[heldTaskStage] = true;

        resetTimer = -1;
    }

    public override void Dropped(Transform character) //These will be RPCed from base pickup class's interact
    {
        base.Dropped(character);

        if (Vector3.Distance(transform.position, deliverPoint) < deliverRadius)
        {
            if (deliverSound)
                audioSource.PlayOneShot(deliverSound);

            //StartCoroutine(DelayDestroy());
            //Destroy(gameObject); 

            reward.LocalReward();
            StartCoroutine(DelayedEvent());
            useable = false;
            CmdReward();
        }
        else if (heldTask)
            heldTask.isComplete[heldTaskStage] = false;

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

    IEnumerator DelayedEvent()
    {
        yield return new WaitForSeconds(cooldown);

        reward.onCompleteDelay.Invoke();
    }
    public override void ResetInteractable()
    {
        base.ResetInteractable();

        resetTimer = 0;
        reward.Reset();
    }
    //  Event Handlers --------------------------------
}