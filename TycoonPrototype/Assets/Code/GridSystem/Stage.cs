using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Stage : MonoBehaviour
{
    public List<BandListingData> currentStagePlaylist;

    public GameObject MainUI;
    public GameObject StageUI;
    public BandDataTransferScript dataTransferScript;
    public LocalAudioHandler LocalAudio;
    public AudioHandler audioHandler;
    public CompositeCollider2D stageCollider;
    public List<Building> audienceAreas;

    public Button quitButton;
    public Button audioButton;
    public bool isPlaying;
    public bool isMouseOverStage = false;

    public GameObject alexObject;
    public GameObject lexieObject;
    public GameObject rockelleObject;
    public GameObject picuObject;

    public Tilemap tilemap;

    public BoundsInt audienceAreaSize;

    private void Start()
    {
        currentStagePlaylist = new List<BandListingData>();
        tilemap = gameObject.GetComponent<Tilemap>();
        stageCollider = gameObject.GetComponent<CompositeCollider2D>();


        LocalAudio = audioButton.GetComponent<LocalAudioHandler>();
        audioHandler = gameObject.GetComponent<AudioHandler>();

        audienceAreas = new List<Building>();

        if (currentStagePlaylist != null)
        {
            dataTransferScript.newbandAdded += UpdateList;
        }

    }

    private void OnMouseEnter()
    {
        isMouseOverStage = true;
        if(EventSystem.current.IsPointerOverGameObject() || StageBuilder.currentInstance.currentActiveStageUI != null || PlaceBandMember.currentInstance.placingBandMember)
        {
            return;
        }

        tilemap.color = new Color(0f, 1f, 0f, 1f);
    }

    private void OnMouseExit()
    {
        isMouseOverStage = false;
        if(!PlaceBandMember.currentInstance.placingBandMember)
        tilemap.color = new Color(1f, 1f, 1f, 1f);

    }


    private void OnMouseDown()
    {
        //When stage is clicked, everything in this function gets executed

        if (EventSystem.current.IsPointerOverGameObject() || StageBuilder.currentInstance.currentActiveStageUI != null || PlaceBandMember.currentInstance.placingBandMember)
        {
            return;
        }

        FillStageUI();

        //Debug.Log(tilemap.CellToWorld(stageCenterTile));


    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(tilemap.CellToWorld(stageCenterTile), 3);

    }

    public void FillStageUI()
    {
        quitButton.onClick.AddListener(ClearStageUI);
        tilemap.color = new Color(1f, 1f, 1f, 1f);

        MainUI.SetActive(false);
        StageUI.SetActive(true);

        LocalAudio.ChangeAudio(audioHandler.stageAudio.mute);
        LocalAudio.ChangeAudioScroll(audioHandler.stageAudio.volume);
        LocalAudio.SoundChange += audioHandler.Mute;
        LocalAudio.volumeChange += audioHandler.SetVolume;

        if (!isPlaying)
        {
            if (currentStagePlaylist.Count == 0)
            {
                dataTransferScript.StartNewLineUp();
                dataTransferScript.ActivateListingUI();
            }
            else
            {
                dataTransferScript.ActivateListingUI();
                dataTransferScript.UploadLineUp(currentStagePlaylist);
            }
            dataTransferScript.playHandeler.playStarted += ActivateCouritine;
            dataTransferScript.playHandeler.playStarted += PlayStageLineup;
        }
        else
        {
            dataTransferScript.ActivatePlayingUI();
        }


        StageBuilder.currentInstance.currentActiveStageUI = this;
    }

    public void UpdateList()
    {
        currentStagePlaylist.Clear();
        foreach (BandListingData data in dataTransferScript.GetNodesList())
        {
            currentStagePlaylist.Add(data);
        }
    }

    public void ClearStageUI() //Function for the playlistUI exit button to call
    {
        if (currentStagePlaylist != null)
        {
            currentStagePlaylist.Clear();
        }
        StartCoroutine(DownloadSongs());

        dataTransferScript.ResetListings();
        
        StopCoroutine(DownloadSongs());
        
        dataTransferScript.playHandeler.playStarted -= ActivateCouritine;

        dataTransferScript.playHandeler.playStarted -= PlayStageLineup;

        LocalAudio.SoundChange -= audioHandler.Mute;
        
        LocalAudio.volumeChange -= audioHandler.SetVolume;
        
        StageBuilder.currentInstance.currentActiveStageUI = null;
        
        quitButton.onClick.RemoveListener(ClearStageUI);
    }

    public void PlayStageLineup()
    {
        audioHandler.LoadMusicFiles();
        audioHandler.Play();
        isPlaying = true;
    }

    private IEnumerator DownloadSongs()
    {
        currentStagePlaylist.Clear();
        Debug.Log("hit");
        foreach (BandListingData data in dataTransferScript.GetNodesList())
        {
            currentStagePlaylist.Add(data);
        }
        yield return new WaitForSeconds(0.05f);
    }

    public void ActivateCouritine()
    {
        StartCoroutine(DownloadSongs());
    }
}
