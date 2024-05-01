using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that has to be attached to each building prefab
public class Building : MonoBehaviour
{
    public bool Placed; //bool for other scripts to check if building has been placed

    public BuildingProperties properties; //slot for scriptable object which holds properties
    public SpriteRenderer image;

    public Vector2 mouseFollowOffset;
    public BoundsInt area; //size of the placement area. ALWAYS keep the z value 1, or else it messes up the calculation

    public Transform NPCTarget;

    private void Start()
    {
        area = properties.placementArea;
    }

    public bool CanBePlaced() //returns whether or not the building can be placed based on the current location on the grid
    {
        Vector3Int positionInt = BuildingSystem.currentInstance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

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
        Placed = true;
        BuildingSystem.currentInstance.TakeArea(areaTemp);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(-mouseFollowOffset, 1);
    }
}
