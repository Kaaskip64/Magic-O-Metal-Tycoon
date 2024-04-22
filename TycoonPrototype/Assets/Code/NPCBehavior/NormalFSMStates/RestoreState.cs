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
            guest.ExecuteCoroutine(EatBurger());
        }
        else if(guest.destinationSetter.target == NPCGlobalData.Instance.beerStand)
        {
            guest.ExecuteCoroutine(DrinkBeer());
        }
        else if(guest.destinationSetter.target == NPCGlobalData.Instance.toilet)
        {
            guest.ExecuteCoroutine(Peeing());
        }
    }
    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
    IEnumerator EatBurger()
    {
        guest.hungryMeter = NPCGlobalData.Instance.initialHungryMeter;
        yield return new WaitForSeconds(NPCGlobalData.Instance.eatTime);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator DrinkBeer()
    {
        guest.thristMeter = NPCGlobalData.Instance.initialThristMeter;
        yield return new WaitForSeconds(NPCGlobalData.Instance.drinkTime);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator Peeing()
    {
        guest.urgencyMeter = NPCGlobalData.Instance.initialUregencyMeter;
        yield return new WaitForSeconds(NPCGlobalData.Instance.peeTime);
        guest.SwitchState(guest.cheerState);
        
    }
}
