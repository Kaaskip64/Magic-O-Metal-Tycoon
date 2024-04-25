using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        guest.GoToTarget(NPCManager.Instance.stage);
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
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
}
