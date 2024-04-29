using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "S_BreakState", menuName = "StatesObject/BreakState")]
public class S_BreakState : S_BaseState
{
    private S_Guest guest;

    public override void EnterState(object obj)
    {
        guest = obj as S_Guest;
        SetDestination();
    }

    public override void ExitState()
    {
        // Nothing to do when exiting the break state
    }

    public override void OnUpdate()
    {
        UpdateMeters();
        CheckDestinationReached();
    }

    private void SetDestination()
    {
        float minMeter = Mathf.Min(guest.hungryMeter, guest.thristMeter, guest.urgencyMeter);
        if (guest.hungryMeter == minMeter)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.foodStands));
        else if (guest.thristMeter == minMeter)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.beerStands));
        else if (guest.urgencyMeter == minMeter)
            guest.GoToTarget(FindClosestBuilding(BuildingSystem.currentInstance.bathroomStands));
    }

    private void UpdateMeters()
    {
        float deltaTime = Time.deltaTime;
        guest.hungryMeter -= NPCManager.Instance.hungryChangeRate / 10 * deltaTime;
        guest.thristMeter -= NPCManager.Instance.thirstChangeRate / 10 * deltaTime;
        guest.urgencyMeter -= NPCManager.Instance.urgencyChangeRate / 10 * deltaTime;
    }

    private void CheckDestinationReached()
    {
        if (guest.aIPath.reachedDestination)
            guest.SwitchState(guest.restoreState);
    }

    private Transform FindClosestBuilding(List<Building> buildingList)
    {
        if (buildingList.Count == 0)
        {
            return null;
        }
        Transform cloestOne;
        cloestOne = buildingList[0].NPCTarget;
        foreach (Building building in buildingList)
        {
            if (Mathf.Abs(Vector3.Distance(guest.transform.position, building.transform.position)) <
                Mathf.Abs(Vector3.Distance(guest.transform.position, cloestOne.transform.position)))
            {
                cloestOne = building.NPCTarget;
            }


        }
        return cloestOne;
    }
}
