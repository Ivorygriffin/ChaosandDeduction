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

    protected int stage = 0;
    [Tooltip("Only one stage will be active at each time")]
    public GameObject[] stages;


    [SerializeField] Reward reward;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();
        //ResetStages();


        //if (reward.interactable)
        //{
        //    reward.interactable.enabled = false;
        //    reward.interactable.Start();
        //}
    }

    protected void OnEnable()
    {
        if (useable)
            ResetStages();
    }

    private void OnDisable()
    {
        if (useable)
            ResetStages();
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
        ChangeStage(true);
        if (stage >= stages.Length - 1) //reached the conclusion
        {
            //award ingredient
            reward.LocalReward();
            CmdReward();

            //script now serves no purpose
            useable = false;
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
        if (reward.task)
        {
            reward.task.isComplete = true;
            TaskManager.instance.CheckTaskComplete();
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

    public void ResetStages()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(stage == i);
        }
    }

    public override void ResetInteractable()
    {
        base.ResetInteractable();

        regressTimer = 0;
        stage = 0;
        ResetStages();
        //if (reward.interactable)
        //    reward.interactable.enabled = false;
        reward.Reset();
    }


    //  Event Handlers --------------------------------
}
