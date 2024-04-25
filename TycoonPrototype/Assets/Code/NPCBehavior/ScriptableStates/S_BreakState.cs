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
            guest.GoToTarget(NPCManager.Instance.buergerKing);
        else if (guest.thristMeter == minMeter)
            guest.GoToTarget(NPCManager.Instance.beerStand);
        else if (guest.urgencyMeter == minMeter)
            guest.GoToTarget(NPCManager.Instance.toilet);
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
}
