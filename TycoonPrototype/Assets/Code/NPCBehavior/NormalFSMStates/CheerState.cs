using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Pipeline;
using UnityEngine;

public class CheerState : BaseState
{
    Guest guest;

    private GameObject stageTarget = null;

    private bool foundAudienceArea = false;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        
<<<<<<< HEAD
        SearchForAudienceArea();
=======
>>>>>>> UUDev
    }

    public override void ExitState()
    {
<<<<<<< HEAD
        foundAudienceArea = false;
=======
        guest.Animator.SetBool("Cheering", false);
        guest.isCheering = false;
>>>>>>> UUDev
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
<<<<<<< HEAD
        if(foundAudienceArea)
        {
            CheerBehaviour();
        }
        else
        {
            SearchForAudienceArea();    
        }
        
=======
        if(Vector2.Distance(guest.transform.position, guest.DestinationSetter.target.position) < 4f && guest.AIPath.reachedDestination)
        {
            guest.isCheering = true;
            guest.Animator.SetBool("Cheering", true);
        }
        CheerBehaviour();
>>>>>>> UUDev
    }

    

    private void SearchForAudienceArea()
    {
        var stage = FindAudienceArea(BuildingSystem.currentInstance.audienceAreas);
        if(stage == null)
        {
            return;
        }
        else
        {
            guest.GoToTarget(stage);
            foundAudienceArea = true;
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
            guest.SwitchState(guest.BreakState);
        }
    }

    private Transform FindAudienceArea(List<Building> stageArea)
    {
        if (stageArea.Count == 0)
        {
            return null;
        }
        
        var targetStage = stageArea[Random.Range(0, stageArea.Count)];

        if (stageTarget == null)
        {
            stageTarget = new GameObject("tempTarget");
        }  
        stageTarget.transform.position = GetRandomPointInCapsule(targetStage.transform.Find("AstarCollider").GetComponent<CapsuleCollider2D>());
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
            float sizeX = collider.size.x / 2 * 100;
            float sizeY = collider.size.y / 2 * 100;

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
