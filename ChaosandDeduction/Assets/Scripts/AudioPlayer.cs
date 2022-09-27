using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource source;

    public void TryPlay()
    {
        if (!source.isPlaying)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
    }


    private void OnValidate()
    {
        source = GetComponent<AudioSource>();
    }
}
