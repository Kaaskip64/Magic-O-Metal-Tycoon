using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.Build.Pipeline.Utilities;

public class BreakState : BaseState
{
    private Guest guest;

    private bool isStateEntered = false;

    private float[] meters = new float[3];

    private int lowMeterCount;

    private bool prepareToLeave;

    private List<Building> availableBuildings = new();

    private float hesitateCount;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        SetDestination();
        isStateEntered = true;
    }

    public override void ExitState()
    {
        lowMeterCount = 0;
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
        meters[0] = guest.hungryMeter;
        meters[1] = guest.thristMeter;
        meters[2] = guest.urgencyMeter;

        Array.Sort(meters);

        for(int i = 0;i<meters.Length;i++)
        {

            Transform currentOne = null;

            if (guest.hungryMeter == meters[i] && guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold)
            {
                currentOne = FindClosestBuilding(BuildingSystem.currentInstance.foodStands);
                if (currentOne != null)
                {
                    guest.GoToTarget(currentOne);
                    break;
                }lowMeterCount++;

            }              
            else if (guest.thristMeter == meters[i] && guest.thristMeter < NPCManager.Instance.thristMeterThreshold)
            {
                currentOne = FindClosestBuilding(BuildingSystem.currentInstance.beerStands);
                if (currentOne != null)
                {
                    guest.GoToTarget(currentOne);
                    break;
                }
                lowMeterCount++;
            }
            else if (guest.urgencyMeter == meters[i] && guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold)
            {
                currentOne = FindClosestBuilding(BuildingSystem.currentInstance.bathroomStands);
                if (currentOne != null)
                {
                    guest.GoToTarget(currentOne);
                    break;
                }
                lowMeterCount++;
            }



        }

        if (lowMeterCount >= 2 && guest.destinationSetter.target == null)
        {
            CheckIfShouldLeave();
        }
        lowMeterCount = 0;
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
            return null;
        }

        foreach(Building build in buildingList)
        {
            var buildingComponent = build.GetComponent<Building>();
            if(buildingComponent.capacityCount<buildingComponent.properties.capacity)
            {
                availableBuildings.Add(buildingComponent);
            }
        }

        if(availableBuildings.Count<=0)
        {
            return null;
        }

        Transform cloestOne;
        cloestOne = availableBuildings[0].NPCTarget;
        foreach(Building building in availableBuildings)
        {
            if(Mathf.Abs(Vector3.Distance(guest.transform.position, building.transform.position))<
                Mathf.Abs(Vector3.Distance(guest.transform.position, cloestOne.transform.position)))
            {
                cloestOne = building.NPCTarget;
            }
        }
        availableBuildings.Clear();
        return cloestOne;
    }

    private void CheckIfShouldLeave()
    {
        if(hesitateCount < NPCManager.Instance.NPCHesitateTime)
        {
            hesitateCount += Time.fixedDeltaTime;
        }
        else
        {
            guest.SwitchState(guest.leaveParkState);
        }
    }
}
