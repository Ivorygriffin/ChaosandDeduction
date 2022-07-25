using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract bool Interact(CharacterInteraction character); //return true to require the player to drop
}
