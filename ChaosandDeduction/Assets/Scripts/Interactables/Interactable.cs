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

    public void localReward()
    {
        if(givenLocalReward)

        givenLocalReward = true;
        if (task)
            task.isComplete = true;
        if (interactable)
            interactable.enabled = true;
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


    //  Unity Methods ---------------------------------



    //  Methods ---------------------------------------
    public void Interact(CharacterInteraction character)
    {
        //Gate keep from characters not aligned with this object
        if ((requiredAlignment != Alignment.Neutral && requiredAlignment != character.alignment))
            return;
        //  || Vector3.Distance(character.transform.position, transform.position) > character.interactionRadius * 2 //TODO: Implement check against interacting too far away

        InteractOverride(character);
    }
    public abstract void InteractOverride(CharacterInteraction character);


    //  Event Handlers --------------------------------
}
