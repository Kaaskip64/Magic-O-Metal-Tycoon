using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

//Handles the placement and management of buildings on grid(s)

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem currentInstance;

    //Grids and Tilemaps to use
    public GridLayout gridLayout;
    public Tilemap MainTileMap; //tilemap to show edit mode/building availability
    public Tilemap TempTileMap; //tilemap where the buildings are hovering

    //Stores basic tiles for visual clarity regarding placement
    public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    //Variables for currently selected building
    private ShopProduct currentSelectedProduct;
    private Building currentSelectedBuilding;
    private Color currentBuildingColor;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    //Mouse
    public Vector3 mousePosOnGrid;
    Ray rayCast;
    RaycastHit hit;

    //Building Lists
    public List<Building> foodStands;
    public List<Building> merchStands;
    public List<Building> beerStands;
    public List<Building> bathroomStands;
    public List<Building> audienceAreas;
    public List<Stage> stages;


    private void Awake()
    {
        currentInstance = this; //init
    }

    private void Start()
    {
        //Adds white, green and red tiles from the resources folder
        //Logic: - white tiles mean unclaimed tiles, this scripts checks placement availability this way
        //       - green tiles mean claimed tiles. Used once a building is placed down
        //       - red tiles mean unavailable tiles. When checking for placement, turns the current placement
        //         selection red, meaning the building cant be placed. Also shows red if tiles are empty, which
        //         means white tiles are essentially also the building bounds
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    private void Update()
    {

        if (!currentSelectedBuilding)
        {
            return;
        }

        //Mouse Position translated to grid position
        mousePosOnGrid = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
            0);
        //raycast
        rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }
        

        if (!currentSelectedBuilding.Placed) //Selected building follows mouse as long as not placed
        {
            Vector3 touchPos = mousePosOnGrid;
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (prevPos != cellPos)
            {
                currentSelectedBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                prevPos = cellPos;
                FollowBuilding(currentSelectedBuilding.area);
            }

        }

        if (Input.GetMouseButtonDown(0)) //Left Mouse Click and checks if temp building can be placed
        {
            if(currentSelectedBuilding && currentSelectedBuilding.CanBePlaced())
            {
                TruePlaceBuilding(); //Places building
                InitializeWithBuilding(currentSelectedProduct);
                currentSelectedBuilding.Placed = false;


            } else if (Physics.Raycast(rayCast, out hit)) //if on click there is no selected building, try to find a new one with raycast
            {
                if (hit.collider.CompareTag("Building"))
                {
                    currentSelectedBuilding = hit.transform.gameObject.GetComponent<Building>();
                    MainTileMap.gameObject.SetActive(true);
                    SetTilesBlock(currentSelectedBuilding.area, TileType.White, MainTileMap);
                    currentSelectedBuilding.Placed = false;
                    currentBuildingColor = new Color(currentBuildingColor.r, currentBuildingColor.g, currentBuildingColor.b, 0.5f);

                }
            }


        }

        if (Input.GetKeyDown(KeyCode.D)) //Removes selected building without placing it
        {
            ClearArea();
            Destroy(currentSelectedBuilding.gameObject);
            MainTileMap.gameObject.SetActive(false);
        }


    }

    public void InitializeWithBuilding(ShopProduct building) //Initialises building at mouse position and follows it
    {
        currentSelectedProduct = building;
        currentSelectedBuilding = Instantiate(building.itemPrefab, mousePosOnGrid, Quaternion.identity).GetComponent<Building>();
        currentSelectedBuilding.gameObject.name = building.ProductName;
        FollowBuilding(currentSelectedBuilding.area);
        MainTileMap.gameObject.SetActive(true);

        currentBuildingColor = currentSelectedBuilding.image.color;
        currentBuildingColor = new Color(currentBuildingColor.r, currentBuildingColor.g, currentBuildingColor.b, 0.5f);
    }

    private void ClearArea() //clears building placement area
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTileMap.SetTilesBlock(prevArea, toClear);
    }

    public void FollowBuilding(BoundsInt currentBuilding) //Makes the placement area below the building follow the selected building
    {
        ClearArea();

        currentBuilding.position = gridLayout.WorldToCell(new Vector3(mousePosOnGrid.x, 
            mousePosOnGrid.y, 0));
        BoundsInt buildingArea = currentBuilding;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTileMap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            } else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTileMap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area) //checks if all tiles in current placement area are white (white tiles mean unclaimed area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTileMap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot be placed!");
                return false;

            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area) //sets the tiles in the current placement area to green
    {
        SetTilesBlock(area, TileType.Empty, TempTileMap);
        SetTilesBlock(area, TileType.Green, MainTileMap);
    }

    public void TruePlaceBuilding()//function for handling all the things that happen once a building is placed
    {
        currentSelectedBuilding.Place();
        PlayerProperties.Instance.ChangeMoney(-currentSelectedProduct.Price);
        currentBuildingColor = new Color(currentBuildingColor.r, currentBuildingColor.g, currentBuildingColor.b, 1f);

        AstarPath.active.data.gridGraph.Scan();

        switch (currentSelectedBuilding.properties.type)
        {
            //switch case to funnel placed building in the corresponding list
            case BuildingProperties.BuildingType.Food:
                foodStands.Add(currentSelectedBuilding.GetComponent<Building>());
                break;

            case BuildingProperties.BuildingType.Beer:
                beerStands.Add(currentSelectedBuilding.GetComponent<Building>());
                break;

            case BuildingProperties.BuildingType.Merch:
                merchStands.Add(currentSelectedBuilding.GetComponent<Building>());
                break;

            case BuildingProperties.BuildingType.Bathroom:
                bathroomStands.Add(currentSelectedBuilding.GetComponent<Building>());
                break;

            case BuildingProperties.BuildingType.Audience:
                audienceAreas.Add(currentSelectedBuilding.GetComponent<Building>());
                currentBuildingColor = new Color(currentBuildingColor.r, currentBuildingColor.g, currentBuildingColor.b, 0.5f);

                break;
        }
    }

    //region for functions that handle tile filling. For some reason, base unity fill commands can crash the editor
    #region TileFillFunctions
    public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    public static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);

    }

    public static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    #endregion


}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
