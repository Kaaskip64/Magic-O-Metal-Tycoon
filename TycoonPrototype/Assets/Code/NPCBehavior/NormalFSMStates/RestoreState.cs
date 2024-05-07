using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RestoreState : BaseState
{
    Guest guest;

    private Building currentBuilding;

    public override void EnterState(object obj)
    {
        guest = obj as Guest;
        
        
        CheckBuilding();
        guest.spriteRenderer.enabled = false;
        guest.GoToTarget(null);
    }
    public override void ExitState()
    {
        guest.spriteRenderer.enabled = true;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    private void CheckBuilding()
    {

        currentBuilding = guest.destinationSetter.target.gameObject.GetComponentInParent<Building>();
        currentBuilding.capacityCount++;
        var buildType = currentBuilding.properties.type;

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
        currentBuilding.capacityCount--;
        Debug.Log(1);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator DrinkBeer()
    {
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.beerPrice);
        guest.thristMeter = NPCManager.Instance.initialThristMeter;
        yield return new WaitForSeconds(NPCManager.Instance.drinkTime);
        currentBuilding.capacityCount--;
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator Peeing()
    {
        PlayerProperties.Instance.ChangeMoney(NPCManager.Instance.toiletPrice);
        guest.urgencyMeter = NPCManager.Instance.initialUregencyMeter;
        yield return new WaitForSeconds(NPCManager.Instance.peeTime);
        currentBuilding.capacityCount--;
        guest.SwitchState(guest.cheerState);
        
    }
}
