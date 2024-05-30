using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Class that has to be attached to each building prefab
public class Building : MonoBehaviour, IHoverPanel
{
    public bool Placed; //bool for other scripts to check if building has been placed

    [Space(20)]

    [Header("Properties")]
    public BuildingType buildingType; //slot for scriptable object which holds properties
    public int capacityMax;
    public int queueCount;
    public BoundsInt area; //size of the placement area. ALWAYS keep the z value 1, or else it messes up the calculation
    public float maintenanceCost = 5f;

    [Space(20)]

    public int capacityCount;
    public SpriteRenderer image;
    public GameObject AstarCollider;

    public Vector2 mouseFollowOffset;

    public Transform NPCTarget;

    private BoxCollider2D boxCollider;



    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(Placed && InformationPanel.instance.currentHoveredBuilding == this)
        {
            if(Input.GetMouseButtonDown(0))
            {
                print("hit");
                boxCollider.enabled = false;
                BuildingSystem.currentInstance.pickingUpBuilding = true;
                BuildingSystem.currentInstance.currentSelectedBuilding = this;
                Placed = false;
                BuildingSystem.currentInstance.MainTileMap.gameObject.SetActive(true);

                
                area.x = BuildingSystem.currentInstance.gridLayout.WorldToCell(gameObject.transform.position).x + 1;
                area.y = BuildingSystem.currentInstance.gridLayout.WorldToCell(gameObject.transform.position).y + 1;

                BuildingSystem.SetTilesBlock(area, TileType.White, BuildingSystem.currentInstance.MainTileMap);

                area.x = 0;
                area.y = 0;

                
                BuildingSystem.currentInstance.FollowBuilding(area);

                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            }
        }
    }

    public bool CanBePlaced() //returns whether or not the building can be placed based on the current location on the grid
    {
        Vector3Int positionInt = BuildingSystem.currentInstance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt + area.position;

        if (BuildingSystem.currentInstance.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place() //When Place() is called, places building
    {
        Vector3Int positionInt = BuildingSystem.currentInstance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        boxCollider.enabled = true;
        Placed = true;
        BuildingSystem.currentInstance.TakeArea(areaTemp);
        
    }


    public void MaintenanceTick()
    {
        PlayerProperties.Instance.MoneyChange(-maintenanceCost);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(-mouseFollowOffset, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InformationPanel.instance.ShowHoverPanel(this);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InformationPanel.instance.HideHoverPanel();
    }
}
