using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioSource menuBack, menuConfirm, menuSelect, playerPickUp, playerItemDrop, playerItemHandOver, playerItemCantInteract, lobbyMusic, mainMenuMusic, gameplayMusic, gameplayVoteMusic, traitorTaskComplete, villagerTaskComplete, votingVoteCast, votingNobodyWins, votingTraitorWins, votingVillagerWins, glueSpurt, pinJar, magicOn, magicOff, taskCompleteStep1, taskCompleteStep2, taskCompleteStep3, traitorCaveScare, traitorGraveLaughMale, traitorGraveLaughFemale, traitorLibraryBurn, traitorPackagingStomp, traitorTreeEtch, traitorWellNPCGrab, traitorWellNPCFall;

    public bool itemHeld;

    public void Start()
    {
        
    }
    public void InteractSound()
    { 
        if (itemHeld == false)
        {
            itemHeld = true;
            playerPickUp.Play();
        }
        else if(itemHeld)
        {
            itemHeld = false;
            playerItemDrop.Play();
        }
       

    }
}
