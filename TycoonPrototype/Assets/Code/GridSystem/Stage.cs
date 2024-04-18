using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
    }

    private void OnMouseEnter()
    {
        tilemap.color = new Color(2f, 2f, 2f, 1f);
    }

    private void OnMouseExit()
    {
        tilemap.color = new Color(1f, 1f, 1f, 1f);

    }

    private void OnMouseDown()
    {
        //When stage is clicked, everything in this function gets executed
        Debug.Log(gameObject.name);

    }

    
}
