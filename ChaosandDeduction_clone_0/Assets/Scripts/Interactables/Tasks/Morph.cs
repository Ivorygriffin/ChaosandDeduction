using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// Replace with comments...
/// </summary>
public class Morph : Interactable
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------



    //  Fields ----------------------------------------
    public bool regressOverTime = false;
    public float regressMaxTimer = 1;
    float regressTimer = 0;

    int stage = 0;
    public GameObject[] stages;

    public GameObject reward;
    bool givenReward = false;
    [Header("This will be toggled on after this morph is completed")]
    public Interactable chainInteractable;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(stage == i);
        }


        if (chainInteractable)
            chainInteractable.enabled = false;
    }


    protected void Update()
    {
        if (regressOverTime && stage > 0)
        {
            regressTimer += Time.deltaTime;
            if (regressTimer > regressMaxTimer)
            {
                ChangeStage(false);
            }
        }
    }


    //  Methods ---------------------------------------
    public override bool Interact(CharacterInteraction character)
    {
        CmdInteract();
        return false;
    }

    [Command(requiresAuthority = false)]
    void CmdInteract()
    {
        RpcInteract();
    }

    [ClientRpc]
    void RpcInteract()
    {
        ChangeStage(true);
        if (stage >= stages.Length - 1) //reached the conclusion
        {
            //award ingredient
            if (reward)
            {
                CmdReward();
            }

            if (chainInteractable)
                chainInteractable.enabled = true;

            //script now serves no purpose
            this.enabled = false; //self disable? hopefully prevents issue of false exploit error
        }
    }

    [Command(requiresAuthority = false)]
    void CmdReward()
    {
        if (givenReward)
            return;

        givenReward = true;
        GameObject temp = Instantiate(reward, transform.position + Vector3.up, Quaternion.identity);
        NetworkServer.Spawn(temp);
    }

    void ChangeStage(bool increase)
    {
        if (stage >= stages.Length - 1)
            return;

        stages[stage].SetActive(false);
        stage += increase ? 1 : -1;
        stages[stage].SetActive(true);

        regressTimer = 0;
    }


    //  Event Handlers --------------------------------
}
