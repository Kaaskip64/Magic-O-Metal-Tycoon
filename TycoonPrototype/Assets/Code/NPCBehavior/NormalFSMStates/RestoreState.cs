using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RestoreState : BaseState
{
    Guest guest;

    public override void EnterState(object obj)
    {
        guest = obj as Guest;

        if(guest.destinationSetter.target == NPCGlobalData.Instance.buergerKing)
        {
            EatBurger();
        }
        else if(guest.destinationSetter.target == NPCGlobalData.Instance.beerStand)
        {
            DrinkBeer();
        }
        else if(guest.destinationSetter.target == NPCGlobalData.Instance.toliet)
        {
            Peeing();
        }
    }
    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    private void EatBurger()
    {
        guest.hungryMeter = NPCGlobalData.Instance.initialHungryMeter;
        guest.SwitchState(guest.cheerState);
    }

    private void DrinkBeer()
    {
        guest.thristMeter = NPCGlobalData.Instance.initialThristMeter;
        guest.SwitchState(guest.cheerState);
    }

    private void Peeing()
    {
        guest.urgencyMeter = NPCGlobalData.Instance.initialUregencyMeter;
        guest.SwitchState(guest.cheerState);
    }
}
