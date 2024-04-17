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
        // ��������ֵ
        guest.hungryMeter -= NPCGlobalData.Instance.hungryChangeRate * Time.deltaTime;
        guest.thristMeter -= NPCGlobalData.Instance.thirstChangeRate * Time.deltaTime;
        guest.urgencyMeter -= NPCGlobalData.Instance.urgencyChangeRate * Time.deltaTime;

        // �ж��Ƿ�������ֵС����ֵ
        bool isHungryLow = guest.hungryMeter < 30;
        bool isThirstLow = guest.thristMeter < 30;
        bool isUrgencyLow = guest.urgencyMeter < 30;

        // ������κ�һ������ֵС����ֵ
        if (isHungryLow || isThirstLow || isUrgencyLow)
        {
            guest.SwitchState(guest.breakState);
        }
    }
}
