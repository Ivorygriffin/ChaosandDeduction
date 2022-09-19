using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Replace with comments...
/// </summary>
public class OneTime : Interactable
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    [SerializeField] Reward reward;



    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();

        //if (reward.interactable)
        //    reward.interactable.enabled = false;
    }


    //  Methods ---------------------------------------
    protected override void InteractOverride(CharacterInteraction character)
    {
        CmdInteract();
    }

    [Command(requiresAuthority = false)]
    void CmdInteract()
    {
        RpcInteract();
    }

    [ClientRpc]
    void RpcInteract()
    {
        //award ingredient
        reward.LocalReward();
        useable = false;
        CmdReward();

        //this script now serves no purpose
        useable = false;
    }

    [Command(requiresAuthority = false)]
    void CmdReward()
    {
        reward.ServerReward(transform);
    }

    public override void ResetInteractable()
    {
        base.ResetInteractable();
        reward.Reset();

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (reward != null)
            reward.drawGizmo(transform);
    }
#endif


    //  Event Handlers --------------------------------
}
