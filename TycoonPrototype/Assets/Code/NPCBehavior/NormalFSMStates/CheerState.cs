using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        guest.GoToTarget(NPCGlobalData.Instance.stage);
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        // 减少属性值
        guest.hungryMeter -= NPCGlobalData.Instance.hungryChangeRate * Time.deltaTime;
        guest.thristMeter -= NPCGlobalData.Instance.thirstChangeRate * Time.deltaTime;
        guest.urgencyMeter -= NPCGlobalData.Instance.urgencyChangeRate * Time.deltaTime;

        // 判断是否有属性值小于阈值
        bool isHungryLow = guest.hungryMeter < 30;
        bool isThirstLow = guest.thristMeter < 30;
        bool isUrgencyLow = guest.urgencyMeter < 30;

        // 如果有任何一个属性值小于阈值
        if (isHungryLow || isThirstLow || isUrgencyLow)
        {
            guest.SwitchState(guest.breakState);
        }
    }
}
