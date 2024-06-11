using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StageBuilder : MonoBehaviour
{
    public static StageBuilder currentInstance;

    [Header("Blank tilemap prefab")]
    public GameObject blankTileMap;

    [Header("Collider Prefab for stage tiles")]
    public GameObject stageAstarCollider;
    public GameObject colliderParent;

    [Header("Tilemap for highlighting box selection")]
    public Tilemap highlightMap;

    [Header("BandDataTransferScript reference")]
    public BandDataTransferScript stageBandData;

    [Header("UI elements")]
    public GameObject MainUI;
    public GameObject StageUI;
    public GameObject buildStageButton;
    public ShopUI shopUI;

    public GameObject upperProductArea;
    public GameObject leaveEraseModeButton;

    public GameObject stageProductArea;
    public GameObject decorationProductArea;
    public GameObject facilitiesProductArea;

    [Header("Size of placement area around tiles")]
    public BoundsInt placementAreaSize;

    [Header("Buttons")]
    public Button quitButton;
    public Button eraseButton;
    public Button soundButton;

    [Header("Active references")]
    public Stage currentActiveStageUI;
    public TileBase currentStageTile;

    [Header("Bools")]

    public bool eraseMode = false;
    public bool placingStageTiles = false;
    public bool editingStage = false;
    public bool placingAudienceAreas = false;
    public bool isDragging = false;

    [Header("Price per tile")]
    public float stageTilePrice;

    public int highlghtTileCount = 0;

    public float currentDragSelectionPrice;

    [HideInInspector]
    public Stage tempStage;
    [HideInInspector]
    public Building audienceAreaPrefab;
    public List<Building> currentStageAudienceAreas;
    private GameObject stageObject;
    private Tilemap stageMap;
    private List<Vector3Int> selectedStageTiles;
    private List<Vector3> allStageTiles;

    private BoundsInt previousBounds;
    private Vector3Int startTilePos;
    private Vector3Int endTilePos;
    private Vector3Int currentTilePos;
    private Vector3Int prevPos;
    private BuildingSystem buildingSystem;

    private void Awake()
    {
        currentInstance = this; //init
        selectedStageTiles = new List<Vector3Int>();
        allStageTiles = new List<Vector3>();
        currentStageAudienceAreas = new List<Building>();
        buildingSystem = BuildingSystem.currentInstance;
        prevPos = Vector3Int.zero;
    }

    private void Update()
    {

        if (!placingStageTiles)
        {
            return;
        }



        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0);
        currentTilePos = stageMap.WorldToCell(new Vector3(mousePos.x, mousePos.y));

        placementAreaSize.x = currentTilePos.x - (placementAreaSize.size.x / 2);
        placementAreaSize.y = currentTilePos.y - (placementAreaSize.size.y / 2);

        currentDragSelectionPrice = stageTilePrice * highlghtTileCount;
        if(prevPos != currentTilePos)
        {
            highlightMap.SetTile(prevPos, null);
            highlightMap.SetTile(currentTilePos, currentStageTile);
            prevPos = currentTilePos;

        }


        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {

            if (!isDragging)
            {
                startTilePos = currentTilePos;
                isDragging = true;

            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            endTilePos = currentTilePos;
            HighlightTiles();
            highlghtTileCount = highlightMap.GetTilesRangeCount(startTilePos, endTilePos);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endTilePos = currentTilePos;
            if (PlayerProperties.Instance.MoneyCheck(currentDragSelectionPrice) && !eraseMode)
            {
                PlayerProperties.Instance.MoneyChange(-currentDragSelectionPrice);
                FillTiles();
            }
            if (eraseMode)
            {
                FillTiles();
                RemoveStageTiles();
                UpdateNoBuildZones();
            }

            selectedStageTiles.Clear();
            highlightMap.ClearAllTiles();
            isDragging = false;
        }

        if(!PlayerProperties.Instance.MoneyCheck(currentDragSelectionPrice))
        {
            highlightMap.color = new Color(1, 0, 0, 0.9f);
        } else
        {
            highlightMap.color = new Color(1, 1, 1, 0.9f);
        }



        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Mouse1)  )
        {
            if(!isDragging)
            {
                InitialiseBuiltStageComponents();
                placingStageTiles = false;

                if(eraseMode)
                {
                    upperProductArea.SetActive(true);
                    leaveEraseModeButton.SetActive(false);
                }

                eraseMode = false;
                editingStage = false;
                placingAudienceAreas = false;
                eraseButton.gameObject.SetActive(false);
                highlightMap.ClearAllTiles();
                shopUI.EnableCategoryButton();
            }
        }

    }

    public void BuildingStage()
    {
        if (!placingStageTiles)
        {
            CreateNewStageObject();
            buildingSystem.MainTileMap.gameObject.SetActive(true);
            buildStageButton.SetActive(false);
            placingStageTiles = true;
            eraseMode = false;
            eraseButton.gameObject.SetActive(true);
            shopUI.DisableCategoryButton();
        }
    }

    public void EraseMode()
    {
        if (!eraseMode)
        {
            upperProductArea.SetActive(false);
            leaveEraseModeButton.SetActive(true);
        } else
        {
            upperProductArea.SetActive(true);
            leaveEraseModeButton.SetActive(false);
        }


        eraseMode = !eraseMode;
    }



    public void SwapCurrentStageTile(TileBase newTile)
    {
        currentStageTile = newTile;
    }

    private void CreateNewStageObject()
    {
        stageObject = Instantiate(blankTileMap);
        stageObject.transform.SetParent(buildingSystem.gridLayout.gameObject.transform);
        stageMap = stageObject.GetComponent<Tilemap>();

        tempStage = stageObject.AddComponent<Stage>();
        tempStage.MainUI = MainUI;
        tempStage.StageUI = StageUI;
        tempStage.dataTransferScript = stageBandData;
        tempStage.quitButton = quitButton;
        tempStage.audioButton = soundButton;

        buildingSystem.stages.Add(tempStage);

    }

    private void InitialiseBuiltStageComponents()
    {
        //initialisation new built stage
        CompositeCollider2D tempComposite = stageObject.AddComponent<CompositeCollider2D>();
        TilemapCollider2D tempTileCol = stageObject.AddComponent<TilemapCollider2D>();

        tempTileCol.usedByComposite = true;

        tempComposite.isTrigger = true;
        tempComposite.attachedRigidbody.isKinematic = true;
        tempComposite.geometryType = CompositeCollider2D.GeometryType.Polygons;

        if(!editingStage)
        {
            AudioSource tempAudioSource = stageObject.AddComponent<AudioSource>();
            AudioHandler tempAudioHandler = stageObject.AddComponent<AudioHandler>();

            tempStage.audioHandler = tempAudioHandler;

            tempStage.gameObject.transform.position = new Vector3(0, 0, -0.01f);
        } else
        {
            StageUI.SetActive(true);
            CameraController.instance.cameraActive = false;
            MainUI.SetActive(false);
            editingStage = false;
        }

        AstarPath.active.data.gridGraph.Scan();
        tempStage = null;
        buildingSystem.MainTileMap.gameObject.SetActive(false);
        buildStageButton.SetActive(true);

    }

    void UpdateNoBuildZones()
    {
        foreach (Stage stage in buildingSystem.stages)
        {
            Tilemap tempMap = stage.tilemap;
            for (int n = tempMap.cellBounds.xMin; n < tempMap.cellBounds.xMax; n++)
            {
                for (int p = tempMap.cellBounds.yMin; p < tempMap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int)tempMap.transform.position.y));
                    Vector3 place = tempMap.CellToWorld(localPlace);
                    if (tempMap.HasTile(localPlace) && localPlace != currentTilePos)
                    {
                        allStageTiles.Add(place);
                    }
                }
            }

            foreach (Vector3 tilePos in allStageTiles)
            {
                placementAreaSize.x = buildingSystem.gridLayout.LocalToCell(tilePos).x - 2;
                placementAreaSize.y = buildingSystem.gridLayout.LocalToCell(tilePos).y - 2;

                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, buildingSystem.MainTileMap);

            }
        }
        allStageTiles.Clear();
    }

    public void RemoveStageTiles()
    {
        foreach(Vector3Int tilepos in selectedStageTiles)
        {
            if (stageMap.HasTile(tilepos))
            {
                stageMap.SetTile(tilepos, null);
                PlayerProperties.Instance.MoneyChange(stageTilePrice);

                placementAreaSize.x = tilepos.x -2;
                placementAreaSize.y = tilepos.y -2;

                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.White, buildingSystem.MainTileMap);

                foreach (Transform child in colliderParent.transform)
                {
                    if (child.position == buildingSystem.gridLayout.CellToLocalInterpolated(tilepos))
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }

    private void HighlightTiles()
    {
        int xMin = Mathf.Min(startTilePos.x, endTilePos.x);
        int xMax = Mathf.Max(startTilePos.x, endTilePos.x);
        int yMin = Mathf.Min(startTilePos.y, endTilePos.y);
        int yMax = Mathf.Max(startTilePos.y, endTilePos.y);

        BoundsInt newBounds = new BoundsInt(new Vector3Int(xMin, yMin, startTilePos.z), new Vector3Int(xMax - xMin + 1, yMax - yMin + 1, 1));


        ClearPreviousBoundsOutliers(previousBounds);

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, startTilePos.z);
                highlightMap.SetTile(tilePos, currentStageTile);
            }
        }
        previousBounds = newBounds;

    }
    private void ClearPreviousBoundsOutliers(BoundsInt bounds)
    {
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, startTilePos.z);
                highlightMap.SetTile(tilePos, null);
            }
        }
    }

    private void FillTiles()
    {

        // Determine the bounds of the rectangle
        int xMin = Mathf.Min(startTilePos.x, endTilePos.x);
        int xMax = Mathf.Max(startTilePos.x, endTilePos.x);
        int yMin = Mathf.Min(startTilePos.y, endTilePos.y);
        int yMax = Mathf.Max(startTilePos.y, endTilePos.y);

        // Set the tiles within the rectangle
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, startTilePos.z);
                if (!eraseMode)
                {
                    stageMap.SetTile(tilePos, currentStageTile);
                    GameObject stageCollider = Instantiate(stageAstarCollider, buildingSystem.gridLayout.CellToLocalInterpolated(tilePos), Quaternion.identity);

                    stageCollider.transform.parent = colliderParent.transform;

                    placementAreaSize.x = tilePos.x - 2;
                    placementAreaSize.y = tilePos.y - 2;

                    BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, buildingSystem.MainTileMap);

                } else
                {
                    if(stageMap.HasTile(tilePos))
                    {
                        selectedStageTiles.Add(tilePos);
                    }
                }
            }
        }

        highlghtTileCount = 0;
    }


    public void SetAudienceAreas(GameObject audienceArea)
    {
        audienceAreaPrefab = audienceArea.GetComponent<Building>();
        Building tempAudienceArea = Instantiate(audienceArea, buildingSystem.mousePosOnGrid, Quaternion.identity).GetComponent<Building>();

        buildingSystem.MainTileMap.gameObject.SetActive(true);
        buildingSystem.currentSelectedBuilding = tempAudienceArea;

        currentStageAudienceAreas = currentActiveStageUI.audienceAreas;
        buildingSystem.FollowBuilding(tempAudienceArea.area, buildingSystem.MainTileMap);

        StageUI.SetActive(false);

        placingAudienceAreas = true;
    }

    public void ClearAudienceAreas()
    {
        if (currentActiveStageUI.audienceAreas != null)
        {
            foreach (Building audienceArea in currentActiveStageUI.audienceAreas)
            {
                Vector3 areaLocation = buildingSystem.gridLayout.WorldToCell(audienceArea.transform.position);
                audienceArea.area.x = (int)areaLocation.x +1;
                audienceArea.area.y = (int)areaLocation.y +1;

                BuildingSystem.SetTilesBlock(audienceArea.area, TileType.White, buildingSystem.MainTileMap);
                Destroy(audienceArea.gameObject);
            }
            currentActiveStageUI.audienceAreas.Clear();
        }

    }

    public void EditStage()
    {
        shopUI.DisableCategoryButton();


        editingStage = true;
        stageObject = currentActiveStageUI.gameObject;
        stageMap = stageObject.GetComponent<Tilemap>();
        StageUI.SetActive(false);
        MainUI.SetActive(true);
        buildStageButton.SetActive(false);
        buildingSystem.MainTileMap.gameObject.SetActive(true);
        placingStageTiles = true;
        eraseMode = false;
        eraseButton.gameObject.SetActive(true);

        Destroy(stageObject.GetComponent<TilemapCollider2D>());
        Destroy(stageObject.GetComponent<CompositeCollider2D>());
        if(facilitiesProductArea.activeInHierarchy)
        {
            facilitiesProductArea.SetActive(false);
            stageProductArea.SetActive(true);
        }

        if(decorationProductArea.activeInHierarchy)
        {
            decorationProductArea.SetActive(false);
            stageProductArea.SetActive(true);
        }

    }
}
