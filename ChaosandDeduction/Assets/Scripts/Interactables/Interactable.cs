using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//  Namespace Properties ------------------------------
[System.Serializable]
public class Reward
{
    [Header("Rewards")]
    [Tooltip("Object that will spawn after task complete")]
    public GameObject item;
    [Tooltip("Task object that will be set to completed upon finish")]
    public TaskScriptableObject task;
    [Tooltip("Chain quest, This will be toggled on after this morph is completed")]
    public Interactable interactable;

    bool givenLocalReward = false;
    public bool givenServerReward = false;

    public void LocalReward() //only allowed to hold local reward here, server reward has to be given out by the script due to networking TODO: figure out if this is true
    {
        if (givenLocalReward)
            return;

        givenLocalReward = true;
        if (task)
        {
            task.isComplete = true;
            //TaskManager.instance.CheckTaskComplete();
        }
        if (interactable)
        {
            interactable.enabled = true;
            interactable.ResetInteractable();
        }
    }

    public void Reset()
    {
        givenLocalReward = false;
        givenServerReward = false;
    }
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

    //  Unity Methods ---------------------------------
    public void Start()
    {
        startPos = transform.position;
        startAngle = transform.rotation;

        if (requiredAlignment == Alignment.Undefined)
            Debug.LogWarning("Alignment on this task has not yet been defined: " + gameObject.name, gameObject);
    }


    //  Methods ---------------------------------------
    public void Interact(CharacterInteraction character)
    {
        //Gate keep from characters not aligned with this object
        if ((requiredAlignment != Alignment.Neutral && requiredAlignment != character.alignment))
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
    //  Event Handlers --------------------------------
}
