using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioSource menuBack, menuConfirm, menuSelect, playerPickUp, playerItemDrop, playerItemHandOver, gameplayMusic, gameplayVoteMusic, traitorTaskCompleteStinger, villagerTaskCompleteStinger, votingVoteCast, votingNobodyWins, votingTraitorWins, votingTraitorWins;

    public bool itemHeld;

    public void Start()
    {
        
    }
    public void InteractSound()
    { 
        if (itemHeld == false)
        {
            itemHeld = true;
            pickUp.Play();
        }
        else if(itemHeld)
        {
            itemHeld = false;
            drop.Play();
        }
       

    }
}
