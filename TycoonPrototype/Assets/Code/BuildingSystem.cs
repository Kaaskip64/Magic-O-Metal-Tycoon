using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTileMap;
    public Tilemap TempTileMap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    private Vector3 mousePosOnGrid;


    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    private void Update()
    {

        if (!temp)
        {
            return;
        }
        mousePosOnGrid = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 0, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if (!temp.Placed)
            {
                Vector3 touchPos = mousePosOnGrid;
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);// + new Vector3(-5f, 0f, 0f);
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
            }
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
        */

        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        if (!temp.Placed)
        {
            Vector3 touchPos = mousePosOnGrid;
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (prevPos != cellPos)
            {
                temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);// + new Vector3(-5f, 0f, 0f);
                prevPos = cellPos;
                FollowBuilding();
            }

        }
        if (Input.GetMouseButtonDown(0) && temp.CanBePlaced())
        {
            temp.Place();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearArea();
            Destroy(temp.gameObject);
        }


    }

    public void InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, mousePosOnGrid, Quaternion.identity).GetComponent<Building>();
        FollowBuilding();
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTileMap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(new Vector3(temp.gameObject.transform.position.x, 1, temp.gameObject.transform.position.z + 5));
        BoundsInt buildingArea = temp.area;

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

    public bool CanTakeArea(BoundsInt area)
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

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTileMap);
        SetTilesBlock(area, TileType.Green, MainTileMap);
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
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

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);

    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }



}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
