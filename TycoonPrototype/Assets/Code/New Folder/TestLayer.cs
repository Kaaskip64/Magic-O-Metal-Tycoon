using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestLayer : MonoBehaviour
{
    public LayerMask buildingLayerMask; // 建筑物所在的图层
    public float distanceThreshold = 1.0f; // 检测距离阈值
    public float yOffset = 1.00f; // 垂直距离偏移量
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
        // 发射射线检测附近的建筑物
/*        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, buildingLayerMask);
*/
        Vector2 rayStartPoint = (Vector2)transform.position - Vector2.up * yOffset;
        Debug.DrawRay(rayStartPoint, Vector2.down * Mathf.Infinity, Color.red);

        // 发射球形射线检测建筑物
        Collider2D[] hits = Physics2D.OverlapCircleAll(rayStartPoint, radius, buildingLayerMask);

        print(hits);



        if (hits.Length > 0)
        {
            // 获取第一个碰撞到的建筑物
            Collider2D hitCollider = hits[0];

            // 获取建筑物的中心点位置
            Vector2 buildingCenter = hitCollider.bounds.center;
            // 获取NPC的中心点位置
            Vector2 npcCenter = spriteRenderer.bounds.center;

            // 计算NPC中心点和建筑物中心点在y轴上的距离
            float distance = npcCenter.y - buildingCenter.y;
            

            // 根据距离调整NPC的sorting order
            if (distance > 0)
            {
                // NPC在建筑物上方，将NPC的sorting order设为比建筑物更高
                spriteRenderer.sortingOrder = hitCollider.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            else
            {
                // NPC在建筑物下方，将NPC的sorting order设为比建筑物更低
                spriteRenderer.sortingOrder = hitCollider.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
        }
    }
}
