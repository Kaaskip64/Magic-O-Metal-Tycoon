using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState : BaseState
{
    Guest guest;
    public override void EnterState(object obj)
    {
        guest = obj as Guest;
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

}
