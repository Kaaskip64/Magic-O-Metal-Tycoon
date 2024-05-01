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
    }


    public void LoadMusicFiles()
    {
        foreach (BandListingData data in stage.currentStagePlaylist)
        {
            audioClips.Add(data.MusicFile);
        }
    }
    public void RemoveEntry()
    {
        audioClips.RemoveAt(0);

        if(audioClips.Count == 0)
        {
            stageAudio.clip = audioClips[0];
            stageAudio.Play();

        }
    }

    public void Play()
    {
        stageAudio.Play();
    }
}
