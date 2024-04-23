using UnityEngine;

public abstract class NPC_FSM : MonoBehaviour
{
    protected BaseState currentState;

    protected abstract void OnEnable();

    protected abstract void Start();

    protected virtual void Update()
    {
        currentState.OnUpdate();
    }

    public virtual void SwitchState(BaseState state)
    {
        if(currentState != null)
        {
            currentState.ExitState();
        }
        currentState = state;
        currentState.EnterState(this);
    }
}
