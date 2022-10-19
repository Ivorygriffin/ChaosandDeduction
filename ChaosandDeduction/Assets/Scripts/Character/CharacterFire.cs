using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

//  Namespace Properties ------------------------------


//  Class Attributes ----------------------------------


/// <summary>
/// The player's interaction component, allowing the player to interact with objects after giving input
/// </summary>
public class CharacterFire : NetworkBehaviour
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public SpellObject loadedSpell;

    //  Fields ----------------------------------------

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        if (loadedSpell)
            loadedSpell.Start();
    }


    protected void Update()
    {
        if (!isLocalPlayer)
            return;

        if (loadedSpell)
            loadedSpell.Update();

        if (FireButton.instance && loadedSpell)
        {
            if ((FireButton.instance.pressedFrame && !loadedSpell.automatic) || (FireButton.instance.pressed && loadedSpell.automatic))
                Fire();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
    }
#endif

    //  Methods ---------------------------------------
    public void Fire()
    {
        if (loadedSpell)
            loadedSpell.Fire(transform.position + transform.forward * 0.5f - transform.up * 0.25f, transform.rotation);
    }

    //  Event Handlers --------------------------------
}
