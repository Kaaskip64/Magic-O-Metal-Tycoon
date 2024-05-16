using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Building))]
public class PlacementAreaVisualiser : Editor
{
    /*
    void OnSceneGUI()
    {
        Building script = (Building)target;
        BoundsInt bounds = script.area;

        Vector3 cellSize = new Vector3(10f, 5f, 1f); // Adjust as per your requirement

        Handles.color = Color.green;

        // Draw isometric gizmos
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            Vector3 worldPos = CalculateIsometricPosition(position, bounds.position, cellSize);
            DrawIsometricTile(worldPos, cellSize);
        }
    }

    Vector3 CalculateIsometricPosition(Vector3Int position, Vector3Int boundsPosition, Vector3 cellSize)
    {
        float x = (position.x - boundsPosition.x) * cellSize.x;
        float y = (position.y - boundsPosition.y) * cellSize.y;

        // Adjust x and y for isometric view
        float isoX = x * 0.5f - y * 0.5f;
        float isoY = x * 0.25f + y * 0.25f;

        Vector3 rotatedPosition = Quaternion.Euler(0, 0, 0) * new Vector3(isoX, isoY, 0f);

        return rotatedPosition;
    }

    void DrawIsometricTile(Vector3 position, Vector3 size)
    {
        Vector3 p1 = position;
        Vector3 p2 = position + new Vector3(0, size.y, 0);
        Vector3 p3 = position + new Vector3(size.x / 2f, size.y / 2f, 0);
        Vector3 p4 = position + new Vector3(size.x / 2f, -size.y / 2f, 0);

        Handles.DrawPolyLine(p1, p3, p2, p4, p1);
        Handles.DrawPolyLine(p1, p2);
        Handles.DrawPolyLine(p3, p4);
    }
    */
}
