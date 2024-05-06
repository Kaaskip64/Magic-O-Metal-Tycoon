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

    public AudioHandler audioHandler;

    public Button quitButton;

    public bool isPlaying;

    private Tilemap tilemap;
    private CompositeCollider2D stageCollider;

    private Vector3 stageCenter;
    private Vector3Int stageCenterTile;

    public BoundsInt audienceAreaSize;

    private void Start()
    {
        currentStagePlaylist = new List<BandListingData>();
        tilemap = gameObject.GetComponent<Tilemap>();
        stageCollider = gameObject.GetComponent<CompositeCollider2D>();
        audioHandler = gameObject.GetComponent<AudioHandler>();

        quitButton.onClick.AddListener(ClearStageUI);

        stageCenter = stageCollider.bounds.center;

        stageCenterTile = tilemap.WorldToCell(stageCenter);

        if (currentStagePlaylist != null)
        {
            dataTransferScript.newbandAdded += UpdateList;

        }

        /*
        audienceAreaSize.position = BuildingSystem.currentInstance.MainTileMap.WorldToCell(stageCenter);

        audienceAreaSize.size.Set(Mathf.RoundToInt(collider.bounds.size.x) , /// BuildingSystem.currentInstance.gridLayout.cellSize.x),
            Mathf.RoundToInt(collider.bounds.size.y) / Mathf.RoundToInt(BuildingSystem.currentInstance.gridLayout.cellSize.y),
            1);

        audienceAreaSize.xMax = Mathf.RoundToInt(collider.bounds.size.x) / Mathf.RoundToInt(BuildingSystem.currentInstance.gridLayout.cellSize.x);
        */
    }

    private void Update()
    {



    }

    private void OnMouseEnter()
    {
        if(EventSystem.current.IsPointerOverGameObject() || StageBuilder.currentInstance.currentActiveStageUI != null)
        {
            return;
        }

        tilemap.color = new Color(0f, 1f, 0f, 1f);
    }

    private void OnMouseExit()
    {
        tilemap.color = new Color(1f, 1f, 1f, 1f);

    }

    private void OnMouseDown()
    {
        //When stage is clicked, everything in this function gets executed

        if (EventSystem.current.IsPointerOverGameObject() || StageBuilder.currentInstance.currentActiveStageUI != null)
        {
            return;
        }

        MainUI.SetActive(false);
        StageUI.SetActive(true);

        if (!isPlaying)
        {
            if (currentStagePlaylist.Count == 0)
            {
                dataTransferScript.StartNewLineUp();
                dataTransferScript.ActivateListingUI();
            }
            else
            {
                dataTransferScript.ActivatePlayingUI();
                dataTransferScript.UploadLineUp(currentStagePlaylist);
            }
            dataTransferScript.playHandeler.playStarted += ActivateCouritine;
            dataTransferScript.playHandeler.playStarted += PlayStageLineup;

        } else
        {
            dataTransferScript.ActivatePlayingUI();
        }

        StageBuilder.currentInstance.currentActiveStageUI = this;

        //Debug.Log(tilemap.CellToWorld(stageCenterTile));


    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(tilemap.CellToWorld(stageCenterTile), 3);

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

        dataTransferScript.playHandeler.playStarted -= ActivateCouritine;

        dataTransferScript.playHandeler.playStarted -= PlayStageLineup;

        DownloadSongs();

        dataTransferScript.ResetListings();

        StageBuilder.currentInstance.currentActiveStageUI = null;
    }

    public void PlayStageLineup()
    {
        audioHandler.LoadMusicFiles();
        audioHandler.Play();


    }

    private IEnumerator DownloadSongs()
    {
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
