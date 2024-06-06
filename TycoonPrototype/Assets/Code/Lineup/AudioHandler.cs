using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource stageAudio;
    public List<AudioClip> audioClips;
    public Stage stage;

    private int indexIndicator = 0;
    private bool listStartPlaying = false;
    private void Start()
    {
        stage = gameObject.GetComponent<Stage>();
        stageAudio = gameObject.GetComponent<AudioSource>();
        audioClips = new List<AudioClip>();
    }
    private void FixedUpdate()
    {
        if (audioClips.Count > 0 && listStartPlaying)
        {
            if (!stageAudio.isPlaying)
            {
                if ((indexIndicator + 1) < audioClips.Count)
                { 
                    Debug.Log(indexIndicator);
                    indexIndicator++;
                    stageAudio.clip = audioClips[indexIndicator];
                    stageAudio.Play();
                }
                else
                {            
                    indexIndicator = 0;
                    listStartPlaying = false;
                    stage.isPlaying = false;
                    stage.currentStagePlaylist.Clear();
                    audioClips.Clear();
                    NPCManager.Instance.UpdateNPCLimit();
                }
            }
        }
    }
    public void LoadMusicFiles()
    {
        foreach (BandListingData data in stage.currentStagePlaylist)
        {
            if (data == null)
            {
                return;
            }
            else
            {
                audioClips.Add(data.MusicFile);
            }
        }
    }

    public void Mute(bool shouldMute)
    {
        stageAudio.mute = shouldMute;
    }

    public void SetVolume(float newVolume)
    {
        stageAudio.volume = newVolume;
    }
    public void Play()
    {
        if (audioClips.Count == 0)
        {
            return;
        }
        stageAudio.clip = audioClips[0];
        stageAudio.Play();
        listStartPlaying = true;
    }
}