using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorting : MonoBehaviour
{
    public LayerMask buildingLayerMask; // Layer where buildings are located
    public float distanceThreshold = 1.0f; // Detection distance threshold
    public float yOffset = 1.00f; // Vertical distance offset
    public float radius = 1.0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found!");
        }
    }

    void Update()
    {
        Vector2 rayStartPoint = (Vector2)transform.position;
        // Debug.DrawRay(rayStartPoint, Vector2.down * Mathf.Infinity, Color.red);

        // Array to store all hits
        List<Collider2D> allHits = new List<Collider2D>();

        // Overlap check for each child object
        foreach (Collider2D Collider in GetComponents<Collider2D>())
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(Collider.transform.position, radius, buildingLayerMask);
            allHits.AddRange(hits);
        }

        // Debug log to check the number of hits
        Debug.Log("Number of hits: " + allHits.Count);

        if (allHits.Count != 0)
        {
            // Get the first collider hit
            Collider2D hitCollider = allHits[0];

            // Get the center position of the building
            Vector2 buildingCenter = hitCollider.bounds.center;
            // Get the center position of the NPC
            Vector2 npcCenter = spriteRenderer.bounds.center;

            // Calculate the distance between the NPC center and building center on the y-axis
            float distance = npcCenter.y - yOffset - buildingCenter.y;
            // Debug.Log("distance between NPC mid and building mid" + distance);

            // Adjust the NPC's sorting order based on the distance
            if (distance > 0)
            {
                // NPC is above the building, set NPC's sorting order lower than the building
                spriteRenderer.sortingOrder = -1;
                print(spriteRenderer.sortingOrder);
            }
            else
            {
                // NPC is below the building, set NPC's sorting order higher than the building
                spriteRenderer.sortingOrder = 1;
                print(spriteRenderer.sortingOrder);
            }
        }
    }
}