using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
    public List<BandListingData> currentStagePlaylist;

    public GameObject MainUI;
    public GameObject StageUI;
    public BandDataTransferScript dataTransferScript;

    private Tilemap tilemap;
    private CompositeCollider2D stageCollider;

    private Vector3 stageCenter;
    private Vector3Int stageCenterTile;

    public BoundsInt audienceAreaSize;
    public bool isPlaying = false;
    private void Start()
    {
        currentStagePlaylist = new List<BandListingData>();
        tilemap = gameObject.GetComponent<Tilemap>();
        stageCollider = gameObject.GetComponent<CompositeCollider2D>();

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
        tilemap.color = new Color(0f, 1f, 0f, 1f);
    }

    private void OnMouseExit()
    {
        tilemap.color = new Color(1f, 1f, 1f, 1f);

    }

    private void OnMouseDown()
    {
        MainUI.SetActive(false);
        StageUI.SetActive(true);
       
        //When stage is clicked, everything in this function gets executed

        MainUI.SetActive(false);
        StageUI.SetActive(true);

        StageBuilder.currentInstance.currentActiveStageUI = this;

        //dataTransferScript.ResetListings();
        if (!isPlaying)
        {
            if(currentStagePlaylist.Count == 0)
            {
                dataTransferScript.StartNewLineUp();
            } else
            {
                dataTransferScript.UploadLineUp(currentStagePlaylist);
            }
            
        }//else(dataTransferScript.)
        


        Debug.Log(gameObject.name);
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
}
