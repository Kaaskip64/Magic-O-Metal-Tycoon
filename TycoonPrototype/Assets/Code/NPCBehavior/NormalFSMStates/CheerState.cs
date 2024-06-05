using System.Collections.Generic;
using UnityEngine;

public class CheerState : BaseState
{
    private Guest guest;
    private GameObject stageTarget;
    private bool foundAudienceArea;
    private readonly List<Building> activeStages = new List<Building>();

    public override void EnterState(object obj)
    {
        guest = obj as Guest;
    }

    public override void ExitState()
    {
        foundAudienceArea = false;
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        CheerBehaviour();
    }

    private void CheerBehaviour()
    {
        guest.hungryMeter -= NPCManager.Instance.hungryChangeRate * Time.fixedDeltaTime;
        guest.thristMeter -= NPCManager.Instance.thirstChangeRate * Time.fixedDeltaTime;
        guest.urgencyMeter -= NPCManager.Instance.urgencyChangeRate * Time.fixedDeltaTime;
        guest.satisfaction += NPCManager.Instance.satisfactionChangeRate * Time.fixedDeltaTime;

        if (NeedsBreak())
        {
            guest.SwitchState(guest.BreakState);
        }
    }

    private bool NeedsBreak()
    {
        return guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold ||
               guest.thristMeter < NPCManager.Instance.thristMeterThreshold ||
               guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold;
    }
}
