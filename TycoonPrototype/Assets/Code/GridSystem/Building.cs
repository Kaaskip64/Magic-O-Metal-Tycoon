using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that has to be attached to each building prefab
public class Building : MonoBehaviour
{
    public bool Placed; //bool for other scripts to check if building has been placed
    public bool mouseHover;
    public BoundsInt area; //size of the placement area. ALWAYS keep the z value 1, or else it messes up the calculation

    public BuildingProperties properties; //slot for scriptable object which holds properties

    public bool CanBePlaced() //returns whether or not the building can be placed based on the current location on the grid
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (BuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place() //When Place() is called, places building
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        BuildingSystem.current.TakeArea(areaTemp);
        
    }

    private void OnMouseOver()
    {
        mouseHover = true;
    }

    private void OnMouseExit()
    {
        mouseHover = false;
    }
}
