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
    //public Button eraseButton;

    [Header("Active references")]
    public Stage currentActiveStageUI;
    public TileBase currentStageTile;

    [Header("Bools")]
    //public bool eraseMode = false;
    public bool editingStageTiles = false;

    [Header("Price per placed stage")]
    public float stagePrice;

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

        placementAreaSize.x = currentTilePos.x - (placementAreaSize.size.x / 2);
        placementAreaSize.y = currentTilePos.y - (placementAreaSize.size.y / 2);

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            stageMap.SetTile(currentTilePos, currentStageTile);
            BuildingSystem.SetTilesBlock(placementAreaSize, TileType.Red, BuildingSystem.currentInstance.MainTileMap);



            //TODO
            //-Economy
        }

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
            }

        } else
        {
            InitialiseBuiltStageComponents();
            editingStageTiles = false;
        }

    }

    public void EraseMode()
    {

        //eraseMode = !eraseMode;
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

    }

    private void InitialiseBuiltStageComponents()
    {
        //initialisation new built stage
        CompositeCollider2D tempComposite = stageObject.AddComponent<CompositeCollider2D>();
        TilemapCollider2D tempTileCol = stageObject.AddComponent<TilemapCollider2D>();
        Stage tempStage = stageObject.AddComponent<Stage>();
        AudioSource tempAudioSource = stageObject.AddComponent<AudioSource>();
        AudioHandler tempAudioHandler = stageObject.AddComponent<AudioHandler>();

        tempTileCol.usedByComposite = true;

        tempComposite.isTrigger = true;
        tempComposite.attachedRigidbody.isKinematic = true;
        tempComposite.geometryType = CompositeCollider2D.GeometryType.Polygons;

        tempStage.MainUI = MainUI;
        tempStage.StageUI = StageUI;
        tempStage.dataTransferScript = stageBandData;
        tempStage.quitButton = quitButton;

        BuildingSystem.currentInstance.stages.Add(tempStage);

        BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(false);

    }
}

public enum TileEditMode
{
    Place,
    Erase
}
