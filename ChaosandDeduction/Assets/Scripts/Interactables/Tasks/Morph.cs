using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
#if UNITY_EDITOR
using System;
#endif

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
    public float[] stageCooldowns;
    float cooldown = 0;

    [SerializeField] Reward reward;


    //  Unity Methods ---------------------------------
    protected void Start()
    {
        base.Start();

#if UNITY_EDITOR
        if (stages.Length != stageCooldowns.Length)
            Debug.LogWarning("Morph does not have correct number of cooldowns");
#endif

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
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }
#if UNITY_EDITOR

    private void OnValidate()
    {
        base.OnValidate();
        try
        {
            if (reward.task != null)
            {
                //Debug.Log(reward.task.name + " " + reward.spawnPoint + " " + (reward.item != null).ToString() + " " + reward.task.paths.Length + " " + (reward.taskStage + 1));
                if (reward.item != null && reward.taskStage + 1 < reward.task.paths.Length)
                {
                    reward.task.paths[reward.taskStage + 1].startPosition = reward.spawnPoint;
                }

                if (gameObject.scene.name != null)
                    reward.task.paths[reward.taskStage].endPosition = transform.position;
            }
        }
        catch (InvalidCastException e)
        {
            Debug.LogError(this.name);
            Debug.LogException(e);
            // recover from exception
        }
    }
#endif


    //  Methods ---------------------------------------
    protected override void InteractOverride(CharacterInteraction character)
    {
        if (cooldown > 0)
            return;
        cooldown = stageCooldowns[stage]; //temp set cooldown (will be set after network RPC)

        if (stage < stages.Length - 1 && stages.Length == 4)
            UIManager.Instance.SetWandProgress(stage + 1);
        else
            UIManager.Instance.ResetWandProgress();

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
            StartCoroutine(DelayedEvent());
            CmdReward();

            //script now serves no purpose
            useable = false;

            //TODO: determine ways to remove this component to save resources
        }
    }

    [Command(requiresAuthority = false)]
    void CmdReward()
    {
        StartCoroutine(DelayedServerRewards(transform));
    }

    IEnumerator DelayedServerRewards(Transform transform)
    {
        yield return new WaitForSeconds(stageCooldowns[stageCooldowns.Length - 1]);

        reward.ServerReward(transform);
    }

    IEnumerator DelayedEvent()
    {
        yield return new WaitForSeconds(stageCooldowns[stageCooldowns.Length - 1]);
        
        reward.onCompleteDelay.Invoke();
    }

    void ChangeStage(bool increase)
    {
        if (stage >= stages.Length - 1)
            return;
        if (stageCooldowns.Length > stage)
            cooldown = stageCooldowns[stage];

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
    public override InteractableType GetInteractableType()
    {
        return InteractableType.Morph;
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
