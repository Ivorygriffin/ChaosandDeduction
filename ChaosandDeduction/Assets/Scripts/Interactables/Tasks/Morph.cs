using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("This will be toggled on after this morph is completed")]
    public Interactable chainInteractable;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        stages[0].SetActive(true);
        for (int i = 1; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
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
        ChangeStage(true);
        if (stage >= stages.Length - 1) //reached the conclusion
        {
            //award ingredient
            if (reward)
                Instantiate(reward, transform.position + Vector3.up, Quaternion.identity);

            if (chainInteractable)
                chainInteractable.enabled = true;

            //script now serves no purpose
            Destroy(this); //only destroy script so the progress remains
        }
        return false;
    }

    void ChangeStage(bool increase)
    {
        stages[stage].SetActive(false);
        stage += increase ? 1 : -1;
        stages[stage].SetActive(true);

        regressTimer = 0;
    }


    //  Event Handlers --------------------------------
}
