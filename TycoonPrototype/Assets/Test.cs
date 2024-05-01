using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Test : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider;
    public GameObject testObj;
    void Start()
    {
        
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 randomPoint = GetRandomPointInCapsule(capsuleCollider);
            Debug.Log("Random Point: " + randomPoint);
            testObj.transform.position = randomPoint;
        }
    }
    Vector2 GetRandomPointInCapsule(CapsuleCollider2D collider)
    {
        Vector2 point = Vector2.zero;
        if (collider != null)
        {
            // 获取Capsule的位置
            Vector2 position = collider.transform.position;

            // 获取Capsule的大小
            float sizeX = collider.size.x * collider.transform.localScale.x / 2;
            float sizeY = collider.size.y * collider.transform.localScale.y / 2;

            // 确定方向
            if (collider.direction == CapsuleDirection2D.Horizontal)
            {
                // 水平方向
                float randomX = Random.Range(-sizeX, sizeX);
                float randomY = Random.Range(-sizeY, sizeY);
                point = new Vector2(position.x + randomX, position.y + randomY);
            }
            else
            {
                // 垂直方向
                float randomX = Random.Range(-sizeY, sizeY);
                float randomY = Random.Range(-sizeX, sizeX);
                point = new Vector2(position.x + randomX, position.y + randomY);
            }
        }

        return point;
    }
}
