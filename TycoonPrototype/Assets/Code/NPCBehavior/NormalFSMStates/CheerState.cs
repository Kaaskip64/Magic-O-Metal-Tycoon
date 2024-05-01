using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class CheerState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        
        guest.GoToTarget(FindAudienceArea(BuildingSystem.currentInstance.audienceAreas));
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        CheerBehaviour();
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

        return targetStage.transform;
    }
}
