using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S_BaseState : ScriptableObject
{
    public abstract void EnterState(object obj);
    public abstract void OnUpdate();
    public abstract void ExitState();
}
