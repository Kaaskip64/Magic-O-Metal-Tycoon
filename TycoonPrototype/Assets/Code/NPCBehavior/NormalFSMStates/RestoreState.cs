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
        guest.SpriteRenderer.enabled = false;
        guest.GoToTarget(null);
    }
    public override void ExitState()
    {
        guest.SpriteRenderer.enabled = true;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    private void CheckBuilding()
    {

        currentBuilding = guest.DestinationSetter.target.gameObject.GetComponentInParent<Building>();
        currentBuilding.capacityCount++;
        var buildType = currentBuilding.buildingType;

        if (buildType == BuildingType.Food)
        {
            guest.StartCoroutine(EatBurger());
        }
        else if (buildType == BuildingType.Beer)
        {
            guest.StartCoroutine(DrinkBeer());
        }
        else if (buildType == BuildingType.Bathroom)
        {
            guest.StartCoroutine(Peeing());
        }
    }


    IEnumerator EatBurger()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.burgerPrice);
        guest.hungryMeter = NPCManager.Instance.initialHungryMeter;
        yield return new WaitForSeconds(NPCManager.Instance.eatTime);
        currentBuilding.capacityCount--;
        Debug.Log(1);
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator DrinkBeer()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.beerPrice);
        guest.thristMeter = NPCManager.Instance.initialThristMeter;
        yield return new WaitForSeconds(NPCManager.Instance.drinkTime);
        currentBuilding.capacityCount--;
        guest.SwitchState(guest.cheerState);
        
    }

    IEnumerator Peeing()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.toiletPrice);
        guest.urgencyMeter = NPCManager.Instance.initialUregencyMeter;
        yield return new WaitForSeconds(NPCManager.Instance.peeTime);
        currentBuilding.capacityCount--;
        guest.SwitchState(guest.cheerState);
        
    }
}
