using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class BreakState : BaseState
{
    private Guest guest;

    private bool isStateEntered = false;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        SetDestination();
        isStateEntered = true;
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        if(!isStateEntered)
        {
            return;
        }

        UpdateMeters();
        SetDestination();

        CheckDestinationReached();
        
    }

    private void SetDestination()
    {
        float minMeter = Mathf.Min(guest.hungryMeter, guest.thristMeter, guest.urgencyMeter);

        if (guest.hungryMeter == minMeter&& guest.hungryMeter<NPCManager.Instance.hungryMeterThreshold)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.foodStands));
        else if (guest.thristMeter == minMeter && guest.thristMeter < NPCManager.Instance.thristMeterThreshold)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.beerStands));
        else if (guest.urgencyMeter == minMeter && guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.bathroomStands));
    }

    private void UpdateMeters()
    {
        float deltaTime = Time.fixedDeltaTime;
        guest.hungryMeter -= NPCManager.Instance.hungryChangeRate / 20 * deltaTime;
        guest.thristMeter -= NPCManager.Instance.thirstChangeRate / 20 * deltaTime;
        guest.urgencyMeter -= NPCManager.Instance.urgencyChangeRate / 20 * deltaTime;
    }

    private void CheckDestinationReached()
    {
        if (guest.destinationSetter.target!=null && Vector2.Distance(guest.transform.position,guest.destinationSetter.target.position)<4f && guest.aIPath.reachedDestination)
        {
            guest.SwitchState(guest.restoreState);
        }    
    }

    private Transform FindClosestBuilding(List<Building> buildingList)
    {
        
        if (buildingList.Count == 0)
        {
            guest.GoToTarget(null);
            return null;
        }
        Transform cloestOne;
        cloestOne = buildingList[0].NPCTarget;
        foreach(Building building in buildingList)
        {
            if(Mathf.Abs(Vector3.Distance(guest.transform.position, building.transform.position))<
                Mathf.Abs(Vector3.Distance(guest.transform.position, cloestOne.transform.position)))
            {
                cloestOne = building.NPCTarget;
            }
        }
        
        return cloestOne;
    }
}
