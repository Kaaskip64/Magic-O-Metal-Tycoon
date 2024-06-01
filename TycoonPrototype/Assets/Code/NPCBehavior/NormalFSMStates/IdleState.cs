using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    Guest guest;

    bool isIdling;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
    }

    public override void ExitState()
    {
        
    }

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
}
