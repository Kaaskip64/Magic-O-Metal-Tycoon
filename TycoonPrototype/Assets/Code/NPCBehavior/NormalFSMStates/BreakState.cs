using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BreakState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        float minMeter = Mathf.Min(guest.hungryMeter, guest.thristMeter, guest.urgencyMeter);
        if (guest.hungryMeter == minMeter)
        {
            guest.GoToTarget(NPCGlobalData.Instance.buergerKing);
        }
        else if (guest.thristMeter == minMeter)
        {
            guest.GoToTarget(NPCGlobalData.Instance.beerStand);
        }
        else if (guest.urgencyMeter == minMeter)
        {
            guest.GoToTarget(NPCGlobalData.Instance.toliet);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        guest.hungryMeter -= NPCGlobalData.Instance.hungryChangeRate/10 * Time.deltaTime;
        guest.thristMeter -= NPCGlobalData.Instance.thirstChangeRate/10 * Time.deltaTime;
        guest.urgencyMeter -= NPCGlobalData.Instance.urgencyChangeRate/10 * Time.deltaTime;

        if (guest.aIPath.reachedDestination)
        {
            guest.SwitchState(guest.restoreState);
        }
    }
}
