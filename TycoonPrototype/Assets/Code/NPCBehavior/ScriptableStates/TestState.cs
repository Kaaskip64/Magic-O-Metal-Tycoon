using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestState", menuName = "StatesObject/Test")]
public class TestState : ScriptableObject 
{
    public string stateName;

    public virtual void EnterState(Guest guest)
    {
        Debug.Log(stateName);
    }
    public virtual void UpdateState(Guest guest) { }
    public virtual void ExitState(Guest guest) { }
}
