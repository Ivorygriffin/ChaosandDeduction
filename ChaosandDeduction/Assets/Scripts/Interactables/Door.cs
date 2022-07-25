using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    [Header("Angle the door will open at and the point it will rotate around")]
    public float angle;
    public Vector3 doorAxis;

    [Header("Time taken to open/close")]
    public float transitionTime = 1.5f;
    float timeTaken = 0;

    bool state = false;
    private void Start()
    {
        doorAxis += transform.position;
    }
    private void Update()
    {
        if (state && timeTaken < transitionTime)
        {
            timeTaken += Time.deltaTime;
            transform.RotateAround(doorAxis, Vector3.up, (angle / transitionTime) * Time.deltaTime);
        }
        if (!state && timeTaken > 0)
        {
            timeTaken -= Time.deltaTime;
            transform.RotateAround(doorAxis, Vector3.up, -(angle / transitionTime) * Time.deltaTime);
        }
    }

    public override bool Interact(CharacterInteraction character)
    {
        state = !state;
        //timeTaken = state ? 0 : transitionTime;
        return false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(doorAxis + transform.position, 0.1f);
    }
#endif
}
