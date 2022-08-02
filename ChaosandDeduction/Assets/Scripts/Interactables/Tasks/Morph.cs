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

    [Header("Regression")]
    [Tooltip("If the morph should go backwards with inaction")]
    public bool regressOverTime = false;
    public float regressMaxTimer = 1;
    float regressTimer = 0;

    int stage = 0;
    [Tooltip("Only one stage will be active at each time")]
    public GameObject[] stages;


    [SerializeField] Reward reward;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(stage == i);
        }


        if (reward.interactable)
            reward.interactable.enabled = false;
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
    public override void InteractOverride(CharacterInteraction character)
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
        ChangeStage(true);
        if (stage >= stages.Length - 1) //reached the conclusion
        {
            //award ingredient
            reward.localReward();
            CmdReward();

            //script now serves no purpose
            this.enabled = false; //self disable? hopefully prevents issue of false exploit error
        }
    }

    [Command(requiresAuthority = false)]
    void CmdReward()
    {
        if (reward.givenServerReward)
            return;
        reward.givenServerReward = true;

        if (reward.item)
        {
            GameObject temp = Instantiate(reward.item, transform.position + Vector3.up, Quaternion.identity);
            NetworkServer.Spawn(temp);
        }
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
