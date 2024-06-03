using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public AudioSource source;

    public List<AudioClip> clips;

    public int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = clips[0];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            PlayNextSongInList();
        }
    }
    
    public void PlayNextSongInList(){

        if (index == clips.Count)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        source.clip = clips[index];
        source.Play(); 
    }
}
