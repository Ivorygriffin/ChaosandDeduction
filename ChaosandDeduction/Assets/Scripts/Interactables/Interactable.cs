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
	[Tooltip("What alignement is required to interact with this script, Neutral means any")]
	public Alignment requiredAlignment = Alignment.Neutral; //Neutral means any


	//  Unity Methods ---------------------------------



	//  Methods ---------------------------------------
	public bool Interact(CharacterInteraction character)
	{
		//Gate keep from characters not aligned with this object
		if (requiredAlignment != Alignment.Neutral && requiredAlignment != character.alignment)
			return false;

		return InteractOverride(character);
	}
	public abstract bool InteractOverride(CharacterInteraction character); //return true to require the player to drop


	//  Event Handlers --------------------------------
}
