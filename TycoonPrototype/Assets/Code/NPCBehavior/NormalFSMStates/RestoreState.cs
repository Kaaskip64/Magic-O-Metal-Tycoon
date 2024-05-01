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
        
        CheckBuilding();
    }
    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    private void CheckBuilding()
    {
        var buildType = guest.destinationTransform.gameObject.GetComponentInParent<Building>().properties.type;

        if (buildType == BuildingProperties.BuildingType.Food)
        {
            guest.StartCoroutine(EatBurger());
        }
        else if (buildType == BuildingProperties.BuildingType.Beer)
        {
            guest.StartCoroutine(DrinkBeer());
        }
        else if (buildType == BuildingProperties.BuildingType.Bathroom)
        {
            guest.StartCoroutine(Peeing());
        }
    }


    IEnumerator EatBurger()
    {
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.burgerPrice);
        guest.hungryMeter = NPCManager.Instance.initialHungryMeter;
        yield return new WaitForSeconds(NPCManager.Instance.eatTime);
        Debug.Log(1);
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
