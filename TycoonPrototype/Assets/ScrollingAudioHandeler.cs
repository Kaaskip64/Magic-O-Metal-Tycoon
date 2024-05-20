using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollingAudioHandeler : MonoBehaviour
{
    public List<AudioClip> clips;
    public AudioSource source;
    public void ChangeAudio(int eventNumber)
    {
        if (!source.isPlaying)
        {
            source.clip = clips[eventNumber];
            source.Play();
        }
    }
}
