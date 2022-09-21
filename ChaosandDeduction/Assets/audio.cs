using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioSource pickUp;
    public AudioSource drop;
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
