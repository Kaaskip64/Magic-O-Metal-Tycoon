using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveParkState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        Debug.Log("Leave");

        guest = (Guest)obj;

        guest.GoToTarget(NPCManager.Instance.spawnPositions[Random.Range(0, NPCManager.Instance.spawnPositions.Length - 1)]);
    }

    public override void ExitState()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
        if(guest.AIPath.reachedDestination && Vector2.Distance(guest.DestinationSetter.transform.position,guest.transform.position)<4)
        {
            NPCManager.Instance.UnregisterNPC(guest);
            GameObject.Destroy(guest.gameObject);
        }
    }

    public override void OnUpdate()
    {
        
    }
}
