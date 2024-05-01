using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General scriptable object to store custom variables for different types of buildings

[CreateAssetMenu(fileName = "Properties", menuName = "ScriptableObjects/Building/BuildingProperties", order = 1)]
public class BuildingProperties : ScriptableObject
{
    public enum BuildingType //sets the building type for reference
    {
        Food,
        Beer,
        Merch,
        Bathroom,
        Stage,
        Audience,
        Deco
    };

    public BuildingType type;

    public BoundsInt placementArea;

    //variables:
    public int queue;
    public int stock;
    public int capacity;

    public float condition;

    

}
