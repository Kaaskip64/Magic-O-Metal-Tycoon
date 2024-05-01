using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class CheerState : BaseState
{
    Guest guest;

    private GameObject stageTarget;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        
        guest.GoToTarget(FindAudienceArea(BuildingSystem.currentInstance.audienceAreas));
    }

    public override void ExitState()
    {
        guest.destinationSetter.target = null;
        GameObject.Destroy(stageTarget);
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        if(guest.destinationSetter.target == null)
        {
            guest.GoToTarget(FindAudienceArea(BuildingSystem.currentInstance.audienceAreas));
        }
        else
        {
            CheerBehaviour();        
        }
        
    }

    private void CheerBehaviour()
    {
        guest.hungryMeter -= NPCManager.Instance.hungryChangeRate * Time.deltaTime;
        guest.thristMeter -= NPCManager.Instance.thirstChangeRate * Time.deltaTime;
        guest.urgencyMeter -= NPCManager.Instance.urgencyChangeRate * Time.deltaTime;

        bool isHungryLow = guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold;
        bool isThirstLow = guest.thristMeter < NPCManager.Instance.thristMeterThreshold;
        bool isUrgencyLow = guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold;

        if (isHungryLow || isThirstLow || isUrgencyLow)
        {
            guest.SwitchState(guest.breakState);
        }
    }

    private Transform FindAudienceArea(List<Building> stageArea)
    {
        if (stageArea.Count == 0)
        {
            return null;
        }

        var targetStage = stageArea[Random.Range(0, stageArea.Count - 1)];

        stageTarget = new GameObject("tempTarget");
        stageTarget.transform.position = GetRandomPointInCapsule(targetStage.GetComponent<CapsuleCollider2D>());
        stageTarget.transform.SetParent(targetStage.transform);
        
        return stageTarget.transform;
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
