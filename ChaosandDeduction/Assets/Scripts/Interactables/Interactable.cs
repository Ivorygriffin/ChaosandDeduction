using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Abstract interactable class intended to allow the player to interact with a variaty of different objects without knowing about each object
/// </summary>
public abstract class Interactable : NetworkBehaviour
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------



	//  Fields ----------------------------------------



	//  Unity Methods ---------------------------------



	//  Methods ---------------------------------------
	public abstract bool Interact(CharacterInteraction character); //return true to require the player to drop


	//  Event Handlers --------------------------------
}
