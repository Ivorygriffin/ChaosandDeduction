using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
//  Namespace Properties ------------------------------
[System.Serializable]
public class Reward
{
    [Header("Rewards")]
    [Tooltip("Object that will spawn after task complete")]
    [Header("Item")]
    public GameObject item;
    [Tooltip("Relative to the task gameobject")]
    public Vector3 spawnPoint;
    [Tooltip("Task object that will be set to completed upon finish")]
    [Header("Task")]
    public TaskScriptableObject task;
    [Tooltip("The stage of the task completed")]
    public int taskStage;
    [Header("Misc")]
    [Tooltip("Chain quest, This will be toggled on after this morph is completed")]
    public Interactable interactable;
    public UnityEvent onComplete;
    public UnityEvent onCompleteDelay;

    bool givenLocalReward = false;
    public bool givenServerReward = false;

    public void LocalReward()
    {
        if (givenLocalReward)
            return;

        givenLocalReward = true;
        if (task)
        {
            task.isComplete[taskStage] = true;
            //TaskManager.instance.CheckTaskComplete();
        }
        if (interactable)
            interactable.Useable();
        onComplete.Invoke();
    }

    public void ServerReward(Transform transform)
    {
        if (givenServerReward)
            return;
        givenServerReward = true;

        if (item)
        {
            GameObject temp = Object.Instantiate(item, spawnPoint, Quaternion.identity);
            NetworkServer.Spawn(temp);
        }
        if (task)
        {
            task.isComplete[taskStage] = true;
            TaskManager.instance.CheckTaskComplete();
        }
    }

    public void Reset()
    {
        givenLocalReward = false;
        givenServerReward = false;
    }

#if UNITY_EDITOR
    public void drawGizmo(Transform transform)
    {
        if (item)
            Gizmos.DrawCube(spawnPoint, Vector3.one);
    }
#endif
}

//  Class Attributes ----------------------------------


/// <summary>
/// Abstract interactable class intended to allow the player to interact with a variaty of different objects without knowing about each object
/// </summary>
public abstract class Interactable : NetworkBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    [Tooltip("What alignement is required to interact with this script, Neutral means any")]
    public Alignment requiredAlignment = Alignment.Neutral; //Neutral means any

    protected Vector3 startPos;
    protected Quaternion startAngle;
    public bool useable = true;

    [Header("Base Sound settings")]
    public AudioSource audioSource;
    public AudioClip interactSound;

    public enum InteractableType
    {
        Interactable,
        Pickup,
        Morph
    }

    //  Unity Methods ---------------------------------
    public void Start()
    {
        startPos = transform.position;
        startAngle = transform.rotation;

        if (requiredAlignment == Alignment.Undefined)
            Debug.LogWarning("Alignment on this task has not yet been defined: " + gameObject.name, gameObject);

        if (interactSound)
            audioSource.PlayOneShot(interactSound);
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
#endif

    //  Methods ---------------------------------------
    public void Interact(CharacterInteraction character)
    {
        //Gate keep from characters not aligned with this object
        if ((requiredAlignment != Alignment.Neutral && requiredAlignment != PlayerManager.Instance.localPlayerData.alignment) || !useable)
            return;
        //  || Vector3.Distance(character.transform.position, transform.position) > character.interactionRadius * 2 //TODO: Implement check against interacting too far away

        InteractOverride(character);
    }
    protected abstract void InteractOverride(CharacterInteraction character);

    public virtual void ResetInteractable()
    {
        transform.position = startPos;
        transform.rotation = startAngle;
    }

    public virtual void Useable()
    {
        useable = true;
        ResetInteractable();
    }

    public virtual InteractableType GetInteractableType()
    {
        return InteractableType.Interactable;
    }

    //  Event Handlers --------------------------------
}
