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
    [SerializeField] private BandDataTransferScript BandListingsList;
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
        if (BandListingsList.GetNodesList()[0].GetComponent<NewBandData>().GetNewBandData() != null)
        {
            if (playStarted != null)
            {
                playStarted(); 
            }
            BandList.SetActive(false);
            MusicList.SetActive(false);
            IsPlayingText.SetActive(true);
        }
        else
        {
            Debug.LogError("No data given");
            return;
        }
    }
    public void EndPlay()
    {
        BandList.SetActive(true);
        MusicList.SetActive(true);
        IsPlayingText.SetActive(false);
    }
}
