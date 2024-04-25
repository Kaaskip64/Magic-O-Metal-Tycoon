using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState : BaseState
{
    private Vector3 initialPosition;
    private float wanderRadius = 5f; 
    private float wanderTimer; 
    private float minWaitTime = 1f; 
    private float maxWaitTime = 2f; 

    private Guest attachedGuest;
    public override void EnterState(object obj)
    {
        Debug.Log("EnterIdleState");

        attachedGuest = (Guest)obj;
        
        initialPosition = attachedGuest.transform.position;

        
        wanderTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    public override void ExitState()
    {

    }

    public override void OnUpdate()
    {
        
        wanderTimer -= Time.deltaTime;
        Wander();

        
        if (wanderTimer <= 0f)
        {
            
            wanderTimer = Random.Range(minWaitTime, maxWaitTime); 
        }
    }

    private void Wander()
    {
        
        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;

        
        Vector3 targetPosition = initialPosition + randomOffset;

        
        float moveSpeed = 5f; 

        
        attachedGuest.transform.position = Vector3.MoveTowards(attachedGuest.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


}
