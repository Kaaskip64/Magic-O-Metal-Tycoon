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

    [Header("Active references")]
    public Stage currentActiveStageUI;
    public TileBase currentStageTile;

    [Header("Bools")]
    public bool eraseMode = false;
    public bool editingStageTiles = false;

    [Header("Price per placed stage")]
    public float stagePrice;

    private GameObject stageObject;
    private Tilemap stageMap;
    private List<Vector3> currentStageTiles;

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

        
        

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            if(!eraseMode)
            {
                stageMap.SetTile(currentTilePos, currentStageTile);
                BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, BuildingSystem.currentInstance.MainTileMap);

            } else
            {
                stageMap.SetTile(currentTilePos, null);

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
        }
        currentStageTiles.Clear();

        if (Input.GetKey(KeyCode.Escape))
        {
            InitialiseBuiltStageComponents();
            editingStageTiles = false;
        }

    }

    public void EditingStage()
    {
        if (!editingStageTiles)
        {
            if (PlayerProperties.Instance.MoneyCheckThenChange(stagePrice))
            {
                CreateNewStageObject();
                PlayerProperties.Instance.ChangeMoney(-stagePrice);
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

        Stage tempStage = stageObject.AddComponent<Stage>();
        tempStage.MainUI = MainUI;
        tempStage.StageUI = StageUI;
        tempStage.dataTransferScript = stageBandData;
        tempStage.quitButton = quitButton;

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


        BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(false);

    }
}

public enum TileEditMode
{
    Place,
    Erase
}
