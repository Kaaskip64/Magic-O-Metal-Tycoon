using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUIAudio : MonoBehaviour
{

    public AudioClip[] audioClips = new AudioClip[2];

    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void PlayAudio(int audioToPlay)
    {
        source.clip = audioClips[audioToPlay];
        source.Play();
    }
}
