using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType : MonoBehaviour
{
    public enum Type
    {
        Food,
        Beer,
        Merch,
        Bathroom
    };

    public Type type;

    public int queue;
    public int stock;

    public float condition;

}
