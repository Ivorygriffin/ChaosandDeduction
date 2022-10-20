using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueBox : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponent<CharacterMovement>().TriggerGlue(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponent<CharacterMovement>().TriggerGlue(false);
    }
}
