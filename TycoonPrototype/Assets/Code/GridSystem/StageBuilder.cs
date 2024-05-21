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

    [Header("Price per placed stage")]
    public float stagePrice;

    public Stage tempStage;
    private GameObject stageObject;
    private Tilemap stageMap;
    private List<Vector3> currentStageTiles;

    private Vector3Int startTilePos;
    private Vector3Int endTilePos;
    private bool isDragging;

    private void Awake()
    {
        currentInstance = this; //init
        currentStageTiles = new List<Vector3>();

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

        placementAreaSize.x = currentTilePos.x - (placementAreaSize.size.x / 2);
        placementAreaSize.y = currentTilePos.y - (placementAreaSize.size.y / 2);

        
        

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && BuildingSystem.currentInstance.currentSelectedBuilding == null)
        {
            
            if(!eraseMode && !isDragging)
            {
                startTilePos = currentTilePos;
                isDragging = true;

            } else
            {
                stageMap.SetTile(currentTilePos, null);
                UpdateNoBuildZones(currentTilePos);
                
            }
        }
        currentStageTiles.Clear();

        if (Input.GetMouseButton(0) && isDragging)
        {
            endTilePos = currentTilePos;
            FillTiles();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endTilePos = currentTilePos;
            FillTiles();
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
                PlayerProperties.Instance.MoneyChange(-stagePrice);
                editingStageTiles = true;
                eraseButton.gameObject.SetActive(true);
            }

        } else
        {
            InitialiseBuiltStageComponents();
            editingStageTiles = false;
            eraseButton.gameObject.SetActive(false);
        }

    }

    public void EraseMode()
    {

        eraseMode = !eraseMode;
    }



    public void SwapCurrentStageTile(TileBase newTile)
    {
        currentStageTile = newTile;
    }

    private void CreateNewStageObject()
    {
        stageObject = Instantiate(blankTileMap);
        stageObject.transform.SetParent(BuildingSystem.currentInstance.gridLayout.gameObject.transform);
        stageMap = stageObject.GetComponent<Tilemap>();

        tempStage = stageObject.AddComponent<Stage>();
        tempStage.MainUI = MainUI;
        tempStage.StageUI = StageUI;
        tempStage.dataTransferScript = stageBandData;
        tempStage.quitButton = quitButton;
        tempStage.audioButton = soundButton;

        BuildingSystem.currentInstance.stages.Add(tempStage);

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
        BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(false);

    }

    void UpdateNoBuildZones(Vector3Int currentTilePos)
    {
        foreach (Stage stage in BuildingSystem.currentInstance.stages)
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
                        currentStageTiles.Add(place);
                    }
                    else if (localPlace == currentTilePos)
                    {
                        placementAreaSize.x = BuildingSystem.currentInstance.gridLayout.WorldToCell(place).x - 2;
                        placementAreaSize.y = BuildingSystem.currentInstance.gridLayout.WorldToCell(place).y - 2;

                        BuildingSystem.SetTilesBlock(placementAreaSize, TileType.White, BuildingSystem.currentInstance.MainTileMap);
                    }
                }
            }

            foreach (Vector3 tilePos in currentStageTiles)
            {
                placementAreaSize.x = BuildingSystem.currentInstance.gridLayout.WorldToCell(tilePos).x - 2;
                placementAreaSize.y = BuildingSystem.currentInstance.gridLayout.WorldToCell(tilePos).y - 2;

                Debug.Log(placementAreaSize.x);

                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, BuildingSystem.currentInstance.MainTileMap);

            }
        }
    }

    void FillTiles()
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
                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, BuildingSystem.currentInstance.MainTileMap);
            }
        }
    }
}
