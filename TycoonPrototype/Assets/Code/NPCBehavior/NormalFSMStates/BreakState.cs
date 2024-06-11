using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
//using UnityEditor.Build.Pipeline.Utilities;

public class BreakState : BaseState
{
    private Guest guest;

    private float[] meters = new float[3];

    private int lowMeterCount;

    private List<Building> availableBuildings = new();

    private float hesitateCount = 0;

    private bool foundFacility;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        SetDestination();
    }

    public override void ExitState()
    {
        lowMeterCount = 0;
        hesitateCount = 0;
        foundFacility = false;
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {

        UpdateMeters();

        if (guest.satisfaction < NPCManager.Instance.satisfactionThreshold)
        {
            guest.SwitchState(guest.leaveParkState);
            return;
        }

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

            if (guest.hungryMeter == meters[i] && guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold)
            {
                if(SetterHelper(BuildingSystem.currentInstance.foodStands))
                {
                    break;
                }
                else
                {
                    guest.StopPathFinding();
                }
            }              
            else if (guest.thristMeter == meters[i] && guest.thristMeter < NPCManager.Instance.thristMeterThreshold)
            {
                if (SetterHelper(BuildingSystem.currentInstance.beerStands))
                {
                    break;
                }
                else
                {
                    guest.StopPathFinding();
                }
            }
            else if (guest.urgencyMeter == meters[i] && guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold)
            {
                if (SetterHelper(BuildingSystem.currentInstance.bathroomStands))
                {
                    break;
                }
                else
                {
                    guest.StopPathFinding();
                }
            }



        }

        if (lowMeterCount >= 2 && guest.DestinationSetter.target == null)
        {
            CheckIfShouldLeave();
        }
        lowMeterCount = 0;
    }

    private bool SetterHelper(List<Building> list)
    {
        var currentOne = FindClosestBuilding(list);
        if (currentOne != null)
        {
            guest.GoToTarget(currentOne);
            foundFacility = true;
            return true;
        }
        foundFacility = false;
        lowMeterCount++;

        return false;
    }


    private void UpdateMeters()
    {
        float deltaTime = Time.fixedDeltaTime;
        guest.hungryMeter -= NPCManager.Instance.hungryChangeRate / 10 * deltaTime;
        guest.thristMeter -= NPCManager.Instance.thirstChangeRate / 10 * deltaTime;
        guest.urgencyMeter -= NPCManager.Instance.urgencyChangeRate / 10 * deltaTime;
        guest.satisfaction -= NPCManager.Instance.satisfactionChangeRate * deltaTime;
    }

    private void CheckDestinationReached()
    {
        if (foundFacility && Vector2.Distance(guest.transform.position,guest.DestinationSetter.target.position)<4f && guest.AIPath.reachedDestination)
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
            if(buildingComponent.capacityCount<buildingComponent.capacityMax)
            {
                availableBuildings.Add(buildingComponent);
            }
        }

        if (availableBuildings.Count<=0)
        {
            return null;
        }

        Transform closestOne;
        closestOne = availableBuildings[0].NPCTarget;
        foreach(Building building in availableBuildings)
        {
            if(Mathf.Abs(Vector3.Distance(guest.transform.position, building.transform.position))<
                Mathf.Abs(Vector3.Distance(guest.transform.position, closestOne.transform.position)))
            {
                closestOne = building.NPCTarget;
            }
        }
        availableBuildings.Clear();

        return closestOne;
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
