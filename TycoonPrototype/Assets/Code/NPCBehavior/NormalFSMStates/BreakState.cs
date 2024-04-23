using UnityEngine;

public class BreakState : BaseState
{
    private Guest guest;

    public override void EnterState(object obj)
    {
        guest = obj as Guest;
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
            guest.GoToTarget(NPCGlobalData.Instance.buergerKing);
        else if (guest.thristMeter == minMeter)
            guest.GoToTarget(NPCGlobalData.Instance.beerStand);
        else if (guest.urgencyMeter == minMeter)
            guest.GoToTarget(NPCGlobalData.Instance.toilet);
    }

    private void UpdateMeters()
    {
        float deltaTime = Time.deltaTime;
        guest.hungryMeter -= NPCGlobalData.Instance.hungryChangeRate / 10 * deltaTime;
        guest.thristMeter -= NPCGlobalData.Instance.thirstChangeRate / 10 * deltaTime;
        guest.urgencyMeter -= NPCGlobalData.Instance.urgencyChangeRate / 10 * deltaTime;
    }

    private void CheckDestinationReached()
    {
        if (guest.aIPath.reachedDestination)
            guest.SwitchState(guest.restoreState);
    }
}
