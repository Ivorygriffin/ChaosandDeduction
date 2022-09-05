using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Deliver deliverable = other.gameObject.GetComponent<Deliver>();

        if (deliverable)
            deliverable.disabledTimer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Deliver deliverable = other.gameObject.GetComponent<Deliver>();

        if (deliverable)
            deliverable.disabledTimer = false;
    }
}
