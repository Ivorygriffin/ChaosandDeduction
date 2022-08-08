using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Alternate version of morph that hides all the stages when disabled rather than just showing the first
/// </summary>
public class HiddenMorph : Morph
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------


    //  Unity Methods ---------------------------------
    private void OnDisable()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
    }


    //  Methods ---------------------------------------



    //  Event Handlers --------------------------------
}
