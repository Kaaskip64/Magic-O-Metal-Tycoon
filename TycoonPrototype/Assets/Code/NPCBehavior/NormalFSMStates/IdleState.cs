using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    Guest guest;
    private GameObject stageTarget;
    private readonly List<Stage> activeStages = new List<Stage>();

    bool isIdling;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        SearchForAudienceArea();
    }

    public override void ExitState() { }

    public override void OnFixedUpdate()
    {
        if(CheckIfAnyStagePlaying())
        {
            guest.SwitchState(guest.cheerState);
        }
        else 
        {
            IdleAround();
        }
    }

    public override void OnUpdate()
    {
        UpdateMeters();
        SearchForAudienceArea();
    }

    private void SearchForAudienceArea()
    {
        if (guest.satisfaction < NPCManager.Instance.satisfactionThreshold)
        {
            guest.SwitchState(guest.leaveParkState);
            return;
        }

        if (NeedsBreak())
        {
            guest.SwitchState(guest.BreakState);
            return;
        }

        var stage = FindActiveAudienceArea(BuildingSystem.currentInstance.stages);
        if (stage != null)
        {
            guest.GoToTarget(stage);
            guest.SwitchState(guest.cheerState);
        }

    }

    private bool NeedsBreak()
    {
        return guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold ||
               guest.thristMeter < NPCManager.Instance.thristMeterThreshold ||
               guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold;
    }

    private void UpdateMeters()
    {
        bool isHungryLow = guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold;
        bool isThirstLow = guest.thristMeter < NPCManager.Instance.thristMeterThreshold;
        bool isUrgencyLow = guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold;

        if (isHungryLow || isThirstLow || isUrgencyLow)
        {
            guest.SwitchState(guest.BreakState);
        }
    }

    private bool CheckIfAnyStagePlaying()
    {
        if(BuildingSystem.currentInstance.stages.Count<=0)
        {
            return false;
        }

        foreach(var stage in BuildingSystem.currentInstance.stages)
        {
            if(stage.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    private void IdleAround()
    {
        if(guest.AIPath.reachedDestination)
        {
            isIdling = false;
        }
        if(isIdling)
        {
            return;
        }

        Transform target = null;

        var temp = Random.value;
        if(temp <=0.7)
        {
            var audienceAreas = BuildingSystem.currentInstance.audienceAreas;
            if (audienceAreas.Count == 0)
            {
                return;
            }
            target = audienceAreas[Random.Range(0, audienceAreas.Count)].transform;
        }
        else
        {
            var merches = BuildingSystem.currentInstance.merchStands;
            if (merches.Count == 0)
            {
                return;
            }

            target = merches[Random.Range(0, merches.Count)].transform;
        }
        if(target != null)
        {
            isIdling = true;
        }
        guest.GoToTarget(target);
    }

    private Transform FindActiveAudienceArea(List<Stage> stageArea)
    {
        activeStages.Clear();
        foreach (var building in stageArea)
        {
            if (building.GetComponent<Stage>().isPlaying)
            {
                activeStages.Add(building);
            }
        }

        //Debug.Log(activeStages.Count);

        if (activeStages.Count == 0)
        {
            return null;
        }
        
        //var targetStage = activeStages[Random.Range(0, activeStages.Count)];
        var targetStage = activeStages[Random.Range(0, activeStages.Count)];
        var audienceAreaOfStage = targetStage.audienceAreas[Random.Range(0, targetStage.audienceAreas.Count)];
        CreateStageTargetIfNeeded();
        stageTarget.transform.position = GetRandomPointInCapsule(audienceAreaOfStage.transform.Find("AstarCollider").GetComponent<CapsuleCollider2D>());
        stageTarget.transform.SetParent(targetStage.transform);

        return stageTarget.transform;
    }

    private void CreateStageTargetIfNeeded()
    {
        if (stageTarget == null)
        {
            stageTarget = new GameObject("tempTarget");
        }
    }

    private Vector2 GetRandomPointInCapsule(CapsuleCollider2D collider)
    {
        if (collider == null) return Vector2.zero;

        Vector2 position = collider.transform.position;
        float sizeX = collider.size.x / 2 * 100;
        float sizeY = collider.size.y / 2 * 100;
        float randomX, randomY;

        if (collider.direction == CapsuleDirection2D.Horizontal)
        {
            randomX = Random.Range(-sizeX, sizeX);
            randomY = Random.Range(-sizeY, sizeY);
        }
        else
        {
            randomX = Random.Range(-sizeY, sizeY);
            randomY = Random.Range(-sizeX, sizeX);
        }

        return new Vector2(position.x + randomX, position.y + randomY);
    }

}
