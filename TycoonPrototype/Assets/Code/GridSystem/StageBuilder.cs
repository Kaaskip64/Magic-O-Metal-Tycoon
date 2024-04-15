using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StageBuilder : MonoBehaviour
{
    public Tilemap groundMap;
    public TileBase currentStageTile;
    public BuildingProperties properties;

    public Button eraseButton;
    public bool eraseMode = false;

    public bool editingStageTiles = false;

    public BoundsInt placementArea;

    private Tilemap mainMap;

    private void Start()
    {
        mainMap = BuildingSystem.current.MainTileMap;
    }

    private void Update()
    {
        if (!editingStageTiles)
        {
            return;
        }

        BuildingSystem.current.MainTileMap.gameObject.SetActive(true);
        

        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
            0);

        Vector3Int currentTile = groundMap.WorldToCell(new Vector3(mousePos.x, mousePos.y));

        placementArea.x = currentTile.x - (placementArea.size.x / 2);
        placementArea.y = currentTile.y - (placementArea.size.y / 2);

        //BuildingSystem.current.FollowBuilding(placementArea);
        

        if (Input.GetMouseButton(0))
        {
            groundMap.SetTile(currentTile, currentStageTile);
            BuildingSystem.SetTilesBlock(placementArea, TileType.Red, mainMap);
        }

    }

    public void EditingStage()
    {

        if(editingStageTiles)
        {

            BuildingSystem.current.MainTileMap.gameObject.SetActive(false);
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
