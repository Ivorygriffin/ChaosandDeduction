using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class TriggerBox : MonoBehaviour
{

    public UnityEvent OnEnter;
  

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            OnEnter.Invoke();
        }
    }
   
}
