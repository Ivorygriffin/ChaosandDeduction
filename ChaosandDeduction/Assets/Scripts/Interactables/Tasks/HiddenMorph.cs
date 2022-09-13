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
    public bool hideInitial = true;
    public bool hideFinal = true;


    //  Unity Methods ---------------------------------
    private void OnEnable()
    {
        base.OnEnable();

        if ((stage <= 0 && !hideInitial) || (stage >= (stages.Length - 1) && !hideFinal)) //ignore the hide effect if its at the start or end and the relevant bool is false
            return;

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
    }


    //  Methods ---------------------------------------



    //  Event Handlers --------------------------------
}
