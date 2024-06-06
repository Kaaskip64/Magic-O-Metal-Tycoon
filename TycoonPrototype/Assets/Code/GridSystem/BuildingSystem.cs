using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//Handles the placement and management of availableBuildings on grid(s)

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem currentInstance;

    //Grids and Tilemaps to use
    [Header("Grids and Tilemaps")]
    public GridLayout gridLayout;
    public Tilemap MainTileMap; //tilemap to show edit mode/building availability
    public Tilemap TempTileMap; //tilemap where the availableBuildings are hovering

    //Stores basic tiles for visual clarity regarding placement
    public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    //Variables for currently selected building
    private ShopProduct currentSelectedProduct;
    public Building currentSelectedBuilding;
    private Color currentBuildingColor;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    //Mouse
    [Header("Mouse")]
    public Vector3 mousePosOnGrid;
    public Vector3Int mouseCellPos;
    //public Ray rayCast;
    public RaycastHit2D hit;

    //Building Lists
    [Header("Placed building lists")]
    public List<Building> foodStands;
    public List<Building> merchStands;
    public List<Building> beerStands;
    public List<Building> bathroomStands;
    public List<Building> audienceAreas;
    public List<Stage> stages;

    //Misc variables
    [Header("Misc variables")]
    public GameObject upperBackgroundShop;

    public UnityAction ExitBuildingFollowing; // handling problem that building placement mouse click can interact with UI elements
    public bool pickingUpBuilding = false;

    private StageBuilder stageBuilder;

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

        stageBuilder = StageBuilder.currentInstance;
    }

    private void Update()
    {
        //Mouse Position translated to grid position
        mousePosOnGrid = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
            0);
        mouseCellPos = gridLayout.LocalToCell(mousePosOnGrid);


        if (!currentSelectedBuilding)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }




        if (!currentSelectedBuilding.Placed) //Selected building no build zone follows mouse as long as not placed
        {
            if (prevPos != mouseCellPos)
            {
                currentSelectedBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(mouseCellPos);
                prevPos = mouseCellPos;
                FollowBuilding(currentSelectedBuilding.area);
            }

        }

        if (Input.GetMouseButtonDown(0)) //Left Mouse Click and checks if temp building can be placed
        {

            if(currentSelectedBuilding && currentSelectedBuilding.CanBePlaced())
            {
                switch(currentSelectedBuilding.buildingType)
                {
                    case BuildingType.Audience:
                        if (currentSelectedBuilding && currentSelectedBuilding.CanBePlaced() && stageBuilder.placingAudienceAreas)
                        {
                            TruePlaceBuilding();
                            currentSelectedBuilding = Instantiate(stageBuilder.audienceAreaPrefab, mousePosOnGrid, Quaternion.identity).GetComponent<Building>();
                            return;
                        }
                        TruePlaceBuilding();

                        if (pickingUpBuilding)
                        {
                            ExitBuildMode();
                            pickingUpBuilding = false;
                        }
                        break;

                    default:
                        if(PlayerProperties.Instance.MoneyCheck(currentSelectedProduct))
                        {
                            TruePlaceBuilding(); //Places building

                            if (pickingUpBuilding)
                            {
                                ExitBuildMode();
                                pickingUpBuilding = false;

                                return;
                            }
                            InitializeWithBuilding(currentSelectedProduct);
                            currentSelectedBuilding.Placed = false;
                        }
                        break;
                }
            } 
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            MirrorBuilding();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey(KeyCode.Mouse1)) //Removes selected building without placing it
        {
            if(stageBuilder.placingAudienceAreas)
            {
                stageBuilder.placingAudienceAreas = false;
                stageBuilder.currentActiveStageUI.audienceAreas = stageBuilder.currentStageAudienceAreas;

                stageBuilder.StageUI.SetActive(true);
            }
            ExitBuildMode();
        }


    }

    public void InitializeWithBuilding(ShopProduct building) //Initialises building at mouse position and follows it
    {
        currentSelectedProduct = building;
        currentSelectedBuilding = Instantiate(building.itemPrefab, mousePosOnGrid, Quaternion.identity).GetComponent<Building>();
        currentSelectedBuilding.image.color = new Color(currentSelectedBuilding.image.color.r, currentSelectedBuilding.image.color.g, currentSelectedBuilding.image.color.b, 0.5f);

        currentSelectedBuilding.gameObject.name = building.ProductName;
        FollowBuilding(currentSelectedBuilding.area);
        upperBackgroundShop.SetActive(false);
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
        print("placed");
        currentSelectedBuilding.image.color = new Color(currentSelectedBuilding.image.color.r, currentSelectedBuilding.image.color.g, currentSelectedBuilding.image.color.b, 1f);



        AstarPath.active.data.gridGraph.Scan();

        if (!pickingUpBuilding)
        {
            if(!stageBuilder.placingAudienceAreas)
            {
                MaintenanceTicks.currentInstance.Tick.AddListener(currentSelectedBuilding.MaintenanceTick);
                PlayerProperties.Instance.MoneyChange(-currentSelectedProduct.Price);

            }
            switch (currentSelectedBuilding.buildingType)
            {
                //switch case to funnel placed building in the corresponding list
                case BuildingType.Food:
                    foodStands.Add(currentSelectedBuilding.GetComponent<Building>());
                    break;

                case BuildingType.Beer:
                    beerStands.Add(currentSelectedBuilding.GetComponent<Building>());
                    break;

                case BuildingType.Merch:
                    merchStands.Add(currentSelectedBuilding.GetComponent<Building>());
                    break;

                case BuildingType.Bathroom:
                    bathroomStands.Add(currentSelectedBuilding.GetComponent<Building>());
                    break;

                case BuildingType.Audience:
                    stageBuilder.currentStageAudienceAreas.Add(currentSelectedBuilding);
                    currentBuildingColor = new Color(currentBuildingColor.r, currentBuildingColor.g, currentBuildingColor.b, 0.5f);

                    break;
            }
        }
    }

    private void ExitBuildMode()
    {
        ClearArea();

        if(!pickingUpBuilding)
        {
            Destroy(currentSelectedBuilding.gameObject);
            upperBackgroundShop.SetActive(true);
        }
        currentSelectedBuilding = null;
        MainTileMap.gameObject.SetActive(false);
        ExitBuildingFollowing();
    }

    private void MirrorBuilding()
    {
        currentSelectedBuilding.area = SwapBoundsValues(currentSelectedBuilding.area);
        FollowBuilding(currentSelectedBuilding.area);

        currentSelectedBuilding.image.flipX = !currentSelectedBuilding.image.flipX;

        Vector2 pos = currentSelectedBuilding.image.gameObject.transform.localPosition;
        pos.x *= -1;

        currentSelectedBuilding.image.gameObject.transform.localPosition = pos;

        Vector2 collScale = currentSelectedBuilding.AstarCollider.transform.localScale;
        Vector2 collPos = currentSelectedBuilding.AstarCollider.transform.localPosition;
        collScale.x *= -1;
        collPos.x *= -1;

        currentSelectedBuilding.AstarCollider.transform.localScale = collScale;
        currentSelectedBuilding.AstarCollider.transform.localPosition = collPos;

        Vector2 entranceScale = currentSelectedBuilding.NPCTarget.localScale;
        Vector2 entrancePos = currentSelectedBuilding.NPCTarget.localPosition;
        entranceScale.x *= -1;
        entrancePos.x *= -1;

        currentSelectedBuilding.NPCTarget.localScale = entranceScale;
        currentSelectedBuilding.NPCTarget.localPosition = entrancePos;

    }

    BoundsInt SwapBoundsValues(BoundsInt bounds)
    {
        return new BoundsInt(
            bounds.x, bounds.y, bounds.z,
            bounds.size.y, bounds.size.x, bounds.size.z
            );
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

public enum BuildingType
{
    Food,
    Beer,
    Merch,
    Bathroom,
    Stage,
    Audience,
    Deco,
    SpawnPoint
};
