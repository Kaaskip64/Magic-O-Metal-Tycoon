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

    [Header("Tilemap for highlighting box selection")]
    public Tilemap highlightMap;

    [Header("BandDataTransferScript reference")]
    public BandDataTransferScript stageBandData;

    [Header("UI elements")]
    public GameObject MainUI;
    public GameObject StageUI;

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
    public bool editingStageTiles = false;
    public bool placingAudienceAreas = false;

    [Header("Price per placed stage")]
    public float stagePrice;

    [HideInInspector]
    public Stage tempStage;
    [HideInInspector]
    public Building audienceAreaPrefab;
    public List<Building> currentStageAudienceAreas;
    private GameObject stageObject;
    private Tilemap stageMap;
    private List<Vector3> surroundingStageTiles;

    private BoundsInt previousBounds;
    private Vector3Int startTilePos;
    private Vector3Int endTilePos;
    private Vector3Int currentTilePos;
    private bool isDragging;
    private BuildingSystem buildingSystem;

    private void Awake()
    {
        currentInstance = this; //init
        surroundingStageTiles = new List<Vector3>();
        currentStageAudienceAreas = new List<Building>();
        buildingSystem = BuildingSystem.currentInstance;
    }

    private void Update()
    {

        if (!editingStageTiles)
        {
            return;
        }



        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0);
        currentTilePos = stageMap.WorldToCell(new Vector3(mousePos.x, mousePos.y));

        placementAreaSize.x = currentTilePos.x - (placementAreaSize.size.x / 2);
        placementAreaSize.y = currentTilePos.y - (placementAreaSize.size.y / 2);




        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {

            if (!eraseMode && !isDragging)
            {
                startTilePos = currentTilePos;
                isDragging = true;

            }
            if (eraseMode)
            {
                stageMap.SetTile(currentTilePos, null);

            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            endTilePos = currentTilePos;
            HighlightTiles();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endTilePos = currentTilePos;
            FillTiles();
            highlightMap.ClearAllTiles();
            isDragging = false;
        }


        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Mouse1))
        {
            InitialiseBuiltStageComponents();
            editingStageTiles = false;
        }

    }

    public void EditingStage()
    {
        if (!editingStageTiles)
        {
            if (PlayerProperties.Instance.MoneyCheck(stagePrice))
            {
                CreateNewStageObject();
                buildingSystem.MainTileMap.gameObject.SetActive(true);
                PlayerProperties.Instance.MoneyChange(-stagePrice);
                editingStageTiles = true;
                eraseMode = false;
                eraseButton.gameObject.SetActive(true);
            }

        }
        else
        {
            InitialiseBuiltStageComponents();
            editingStageTiles = false;
            eraseButton.gameObject.SetActive(false);
        }

    }

    public void EraseMode()
    {
        if (eraseMode)
        {
            UpdateNoBuildZones();
            surroundingStageTiles.Clear();
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
        AudioSource tempAudioSource = stageObject.AddComponent<AudioSource>();
        AudioHandler tempAudioHandler = stageObject.AddComponent<AudioHandler>();


        tempTileCol.usedByComposite = true;

        tempComposite.isTrigger = true;
        tempComposite.attachedRigidbody.isKinematic = true;
        tempComposite.geometryType = CompositeCollider2D.GeometryType.Polygons;

        tempStage.audioHandler = tempAudioHandler;

        tempStage.gameObject.transform.position = new Vector3(0, 0, -0.01f);

        tempStage = null;
        buildingSystem.MainTileMap.gameObject.SetActive(false);

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
                        surroundingStageTiles.Add(place);
                    }
                    else if (localPlace == currentTilePos)
                    {
                        placementAreaSize.x = buildingSystem.gridLayout.WorldToCell(place).x - 2;
                        placementAreaSize.y = buildingSystem.gridLayout.WorldToCell(place).y - 2;

                        BuildingSystem.SetTilesBlock(placementAreaSize, TileType.White, buildingSystem.MainTileMap);
                        Debug.Log("hit");
                    }
                }
            }

            foreach (Vector3 tilePos in surroundingStageTiles)
            {
                placementAreaSize.x = buildingSystem.gridLayout.WorldToCell(tilePos).x - 2;
                placementAreaSize.y = buildingSystem.gridLayout.WorldToCell(tilePos).y - 2;

                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, buildingSystem.MainTileMap);

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
                stageMap.SetTile(tilePos, currentStageTile);

                placementAreaSize.x = tilePos.x - 2;
                placementAreaSize.y = tilePos.y - 2;

                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, buildingSystem.MainTileMap);
            }
        }
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
}
