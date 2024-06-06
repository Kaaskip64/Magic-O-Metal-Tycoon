using System.Collections.Generic;
using UnityEngine;

public class CheerState : BaseState
{
    private Guest guest;
    private GameObject stageTarget;
    private readonly List<Building> activeStages = new List<Building>();

    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        
    }

    public override void ExitState()
    {
        guest.Animator.SetBool("Cheering", false);
        guest.isCheering = false;
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        if(Vector2.Distance(guest.transform.position, guest.DestinationSetter.target.position) < 4f && guest.AIPath.reachedDestination)
        {
            guest.isCheering = true;
            guest.Animator.SetBool("Cheering", true);
        }
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

        if(NoActiveStage())
        {
           guest.SwitchState(guest.idleState); 
        }
    }

    private bool NeedsBreak()
    {
        return guest.hungryMeter < NPCManager.Instance.hungryMeterThreshold ||
               guest.thristMeter < NPCManager.Instance.thristMeterThreshold ||
               guest.urgencyMeter < NPCManager.Instance.uregencyMeterThreshold;
    }

    private bool NoActiveStage()
    {
        if (BuildingSystem.currentInstance.stages.Count <= 0)
        {
            return true;
        }

        foreach (var stage in BuildingSystem.currentInstance.stages)
        {
            if (stage.isPlaying)
            {
                return false;
            }
        }
        return true;
    }
}
