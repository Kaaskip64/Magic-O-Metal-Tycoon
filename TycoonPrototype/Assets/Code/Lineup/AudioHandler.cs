using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource stageAudio;
    public List<AudioClip> audioClips;
    public Stage stage;

    private void Start()
    {
        stage = gameObject.GetComponent<Stage>();
        stageAudio = gameObject.GetComponent<AudioSource>();
        audioClips = new List<AudioClip>();
    }


    public void LoadMusicFiles()
    {
        foreach (BandListingData data in stage.currentStagePlaylist)
        {
            Debug.Log(data.MusicFile);
            audioClips.Add(data.MusicFile);
        }
    }
    public void RemoveEntry()
    {
        audioClips.RemoveAt(0);

        if(audioClips.Count == 0)
        {
            Play();

        }
    }

    public void Play()
    {
        stageAudio.clip = audioClips[0];
        stageAudio.Play();
    }
}
