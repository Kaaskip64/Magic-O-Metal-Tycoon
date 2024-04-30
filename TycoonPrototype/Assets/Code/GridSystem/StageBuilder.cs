using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StageBuilder : MonoBehaviour
{
    public static StageBuilder currentInstance;

    public GameObject blankTileMap;
    public TileBase currentStageTile;
    public BuildingProperties properties;
    public GameObject shopStageButton;
    public GameObject MainUI;
    public GameObject StageUI;
    public BandDataTransferScript stageBandData;
    public Stage currentActiveStageUI;

    public Button eraseButton;
    public bool eraseMode = false;

    public bool editingStageTiles = false;

    public BoundsInt placementArea;

    private GameObject stageObject;
    private Tilemap stageMap;

    private void Awake()
    {
        currentInstance = this; //init
    }

    private void Update()
    {

        if (!editingStageTiles)
        {
            return;
        }


        BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(true);

        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
            0);
        Vector3Int currentTilePos = stageMap.WorldToCell(new Vector3(mousePos.x, mousePos.y));

        placementArea.x = currentTilePos.x - (placementArea.size.x / 2);
        placementArea.y = currentTilePos.y - (placementArea.size.y / 2);

        //BuildingSystem.current.FollowBuilding(placementArea);
        

        if (Input.GetMouseButton(0))
        {
            stageMap.SetTile(currentTilePos, currentStageTile);
            BuildingSystem.SetTilesBlock(placementArea, TileType.Red, BuildingSystem.currentInstance.MainTileMap);
            //TODO
            //-Economy
        }

    }

    public void EditingStage()
    {

        if(editingStageTiles)
        {
            //initialisation new built stage
            CompositeCollider2D tempComposite =  stageObject.AddComponent<CompositeCollider2D>();
            TilemapCollider2D tempTileCol = stageObject.AddComponent<TilemapCollider2D>();
            Stage tempStage = stageObject.AddComponent<Stage>();

            tempTileCol.usedByComposite = true;

            tempComposite.isTrigger = true;
            tempComposite.attachedRigidbody.isKinematic = true;
            tempComposite.geometryType = CompositeCollider2D.GeometryType.Polygons;
            tempStage.MainUI = MainUI;
            tempStage.StageUI = StageUI;
            tempStage.dataTransferScript = stageBandData;
            BuildingSystem.currentInstance.stages.Add(tempStage);


            BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(false);
        } else
        {
            stageObject = Instantiate(blankTileMap);
            stageObject.transform.SetParent(BuildingSystem.currentInstance.gridLayout.gameObject.transform);
            stageMap = stageObject.GetComponent<Tilemap>();
        }


        editingStageTiles = !editingStageTiles;

    }

    public void EraseMode()
    {

        eraseMode = !eraseMode;
    }

    public void ClearStageUI() //Function for the playlistUI exit button to call
    {
        if (currentActiveStageUI.currentStagePlaylist != null)
        {
            //currentActiveStageUI.currentStagePlaylist.Clear();
        }

        foreach (BandListingData data in stageBandData.GetNodesList())
        {
            Debug.Log(data);
            currentActiveStageUI.currentStagePlaylist.Add(data);
        }

        currentActiveStageUI = null;
    }
}

public enum TileEditMode
{
    Place,
    Erase
}
