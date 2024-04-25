using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StageBuilder : BuildingSystem
{
    public GameObject blankTileMap;
    public TileBase currentStageTile;
    public BuildingProperties properties;
    public GameObject shopStageButton;

    public Button eraseButton;
    public bool eraseMode = false;

    public bool editingStageTiles = false;

    public BoundsInt placementArea;

    private GameObject stageObject;
    private Tilemap stageMap;

    private void Update()
    {

        if (!editingStageTiles)
        {
            return;
        }


        currentInstance.MainTileMap.gameObject.SetActive(true);

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
            SetTilesBlock(placementArea, TileType.Red, MainTileMap);
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
            currentInstance.placedBuildings.stages.Add(tempStage);
            

            currentInstance.MainTileMap.gameObject.SetActive(false);
        } else
        {
            stageObject = Instantiate(blankTileMap);
            stageObject.transform.SetParent(gridLayout.gameObject.transform);
            stageMap = stageObject.GetComponent<Tilemap>();
        }


        editingStageTiles = !editingStageTiles;

    }

    public void EraseMode()
    {

        eraseMode = !eraseMode;
    }
}

public enum TileEditMode
{
    Place,
    Erase
}
