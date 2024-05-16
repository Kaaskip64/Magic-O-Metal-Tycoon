using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "S_RestoreState", menuName = "StatesObject/RestoreState")]
public class S_RestoreState : S_BaseState
{
    S_Guest guest;

    public override void EnterState(object obj)
    {
        guest = obj as S_Guest;

        var buildType = guest.destinationSetter.target.gameObject.GetComponent<Building>().properties.type;

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
    public override void ExitState()
    {

    }

    public override void OnUpdate()
    {

    }
    IEnumerator EatBurger()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.burgerPrice);
        guest.hungryMeter = NPCManager.Instance.initialHungryMeter;
        yield return new WaitForSeconds(NPCManager.Instance.eatTime);
        guest.SwitchState(guest.cheerState);

    }

    IEnumerator DrinkBeer()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.beerPrice);
        guest.thristMeter = NPCManager.Instance.initialThristMeter;
        yield return new WaitForSeconds(NPCManager.Instance.drinkTime);
        guest.SwitchState(guest.cheerState);

    }

    IEnumerator Peeing()
    {
        PlayerProperties.Instance.MoneyChange(NPCManager.Instance.toiletPrice);
        guest.urgencyMeter = NPCManager.Instance.initialUregencyMeter;
        yield return new WaitForSeconds(NPCManager.Instance.peeTime);
        guest.SwitchState(guest.cheerState);

    }
}
