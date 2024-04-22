using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
    public List<Song> currentStagePlaylist;

    private Tilemap tilemap;
    private CompositeCollider2D collider;

    private Vector3 stageCenter;
    private Vector3Int stageCenterTile;

    public BoundsInt audienceAreaSize;

    private void Start()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
        collider = gameObject.GetComponent<CompositeCollider2D>();

        stageCenter = collider.bounds.center;

        stageCenterTile = tilemap.WorldToCell(stageCenter);

        audienceAreaSize.position = BuildingSystem.currentInstance.MainTileMap.WorldToCell(stageCenter);

        audienceAreaSize.size.Set(Mathf.RoundToInt(collider.bounds.size.x) , /// BuildingSystem.currentInstance.gridLayout.cellSize.x),
            Mathf.RoundToInt(collider.bounds.size.y) / Mathf.RoundToInt(BuildingSystem.currentInstance.gridLayout.cellSize.y),
            1);

        //audienceAreaSize.xMax = Mathf.RoundToInt(collider.bounds.size.x) / Mathf.RoundToInt(BuildingSystem.currentInstance.gridLayout.cellSize.x);

    }

    private void Update()
    {



    }

    private void OnMouseEnter()
    {
        tilemap.color = new Color(0f, 1f, 0f, 1f);
    }

    private void OnMouseExit()
    {
        tilemap.color = new Color(1f, 1f, 1f, 1f);

    }

    private void OnMouseDown()
    {
        //When stage is clicked, everything in this function gets executed
        Debug.Log(gameObject.name);
        Debug.Log(tilemap.CellToWorld(stageCenterTile));
        Debug.Log(Mathf.RoundToInt(collider.bounds.size.x));
        Debug.Log(BuildingSystem.currentInstance.gridLayout.cellSize.x);


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(tilemap.CellToWorld(stageCenterTile), 3);

    }
}
