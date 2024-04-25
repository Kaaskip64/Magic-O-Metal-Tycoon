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

        if(guest.destinationSetter.target == NPCManager.Instance.buergerKing)
        {
            guest.StartCoroutine(EatBurger());
        }
        else if(guest.destinationSetter.target == NPCManager.Instance.beerStand)
        {
            guest.StartCoroutine(DrinkBeer());
        }
        else if(guest.destinationSetter.target == NPCManager.Instance.toilet)
        {
            guest.StartCoroutine(Peeing());
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
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.burgerPrice);
        guest.hungryMeter = NPCManager.Instance.initialHungryMeter;
        yield return new WaitForSeconds(NPCManager.Instance.eatTime);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator DrinkBeer()
    {
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.beerPrice);
        guest.thristMeter = NPCManager.Instance.initialThristMeter;
        yield return new WaitForSeconds(NPCManager.Instance.drinkTime);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator Peeing()
    {
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.toiletPrice);
        guest.urgencyMeter = NPCManager.Instance.initialUregencyMeter;
        yield return new WaitForSeconds(NPCManager.Instance.peeTime);
        guest.SwitchState(guest.cheerState);
        
    }
}
