using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

public class CharacterInteraction : MonoBehaviour
{
    public float interactionRadius = 1;
    Interactable currentInteraction = null;

    void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current[Key.E].wasReleasedThisFrame)
        {
            Interact();
        }
#endif
    }

    public void Interact()
    {
        if (!currentInteraction)
        {
            Collider[] Results = Physics.OverlapSphere(transform.position, interactionRadius);
            for (int i = 0; i < Results.Length; i++)
            {
                if (Results[i] != null)
                {
                    Interactable interactable = Results[i].GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        if (interactable.Interact(this)) //if is only used for checking if the interaction should be kept, it will be interacted with either way here
                            currentInteraction = interactable;
                        break; //only trigger first interaction
                               //TODO: Determine if i need to do the closest interactable or if the list is already sorted
                    }
                }
            }

        }
        else
        {
            if (!currentInteraction.Interact(this)) // check if we can forget about the object
                currentInteraction = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube((transform.position + (transform.forward)), Vector3.one * 0.2f);
    }
}
