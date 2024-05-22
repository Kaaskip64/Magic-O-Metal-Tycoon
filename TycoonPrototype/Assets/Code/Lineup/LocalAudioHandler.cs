using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalAudioHandler : MonoBehaviour
{


    public bool soundActive = true;
    public Button button;
    public Image image;
    public List<Sprite> onOff;
    public Scrollbar scrollBar;
    public float volume;
    public delegate void SoundActivationChanged(bool SoundActive);

    public SoundActivationChanged SoundChange;

    public delegate void SoundVolumeChanged(float changedVolume);

    public SoundVolumeChanged volumeChange;
    public void ChangeAudio(bool isOn)
    {
        soundActive = isOn;
        changeSprite(isOn);
        if (SoundChange != null)
        {
            SoundChange(soundActive);
        }
    }
    public void ChangeAudio()
    {
        soundActive = !soundActive;
        ChangeAudioScroll();
        changeSprite(soundActive);
        if (SoundChange != null)
        {
            SoundChange(soundActive);
        }
    }

    private void changeSprite(bool active)
    {
        if (active)
        {
            image.sprite = onOff[0];
        }
        else
        {
            image.sprite = onOff[1];
        }
    }

    public void ChangeAudioScroll()
    {
        volume = scrollBar.value;
        volumeChange(volume);
    }

    public void ChangeAudioScroll(float audioVolume)
    {
        volume = audioVolume;
        scrollBar.value = volume;
        volumeChange(volume);
    }
}
