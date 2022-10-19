using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;
    public AudioPlayer footsteps;
    const float requiredFootstepSpeed = 0.2f;

    Vector3 lastFramePos = Vector3.zero;

    // Update is called once per frame
    public void RemoteUpdate()
    {
        float speed = Vector3.Distance(lastFramePos, transform.position) / (Time.deltaTime);
        animator.SetFloat("Blend", speed);

        if (speed > requiredFootstepSpeed && footsteps)
            footsteps.TryPlay();

        lastFramePos = transform.position;
    }
    public void LocalUpdate(Vector3 velocity)
    {
        if (animator)
            animator.SetFloat("Blend", velocity.magnitude);

        if (velocity.magnitude > requiredFootstepSpeed && footsteps)
            footsteps.TryPlay();
    }
}
