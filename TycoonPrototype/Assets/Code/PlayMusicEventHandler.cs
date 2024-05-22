using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayMusicEventHandler : MonoBehaviour
{
    public delegate void PlayStarted();
    public PlayStarted playStarted;
    [SerializeField] private BandDataTransferScript bandListingsList;
    [SerializeField] private Button button;
    [SerializeField] private GameObject BandList;
    [SerializeField] private GameObject MusicList;
    [SerializeField] private GameObject IsPlayingText;
    // Start is called before the first frame update
    void Start()
    {
       button.onClick.AddListener(StartPlay);
    }
    public void StartPlay()
    {
        if (bandListingsList.GetNodesList()[0] != null) 
        {
            if (playStarted != null)
            {
                playStarted(); 
            }
            BandList.SetActive(false);
            MusicList.SetActive(false);
            IsPlayingText.SetActive(true);
            button.enabled = false;
        }
        
    }
    public void EndPlay()
    {
        BandList.SetActive(true);
        //MusicList.SetActive(true);
        IsPlayingText.SetActive(false);
        button.enabled = true;
    }
}
