using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Properties", menuName = "ScriptableObjects/Building/BuildingProperties", order = 1)]
public class BuildingProperties : ScriptableObject
{
    public enum BuildingType
    {
        Food,
        Beer,
        Merch,
        Bathroom
    };

    public BuildingType type;

    public int queue;
    public int stock;
    public int capacity;

    public float condition;

    

}
