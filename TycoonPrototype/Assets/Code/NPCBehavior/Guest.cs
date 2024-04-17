using Pathfinding;
using UnityEngine;

public class Guest : NPC_FSM
{
    [HideInInspector]
    public AIDestinationSetter destinationSetter;
    [HideInInspector]
    public AIPath aIPath;

    private Transform destinationTransform;

    public IdleState idleState;
    public BreakState breakState;
    public CheerState cheerState;
    public RestoreState restoreState;

    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;

    [HideInInspector]
    public TestState testState;
    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        idleState = new();
        cheerState = new();
        breakState = new();
        restoreState = new(); 
    }

    protected override void Start()
    {
        hungryMeter = NPCGlobalData.Instance.initialHungryMeter;
        thristMeter = NPCGlobalData.Instance.initialThristMeter;
        urgencyMeter = NPCGlobalData.Instance.initialUregencyMeter;
        satisfaction = NPCGlobalData.Instance.initialSatisfaction;
        SwitchState(cheerState);
    }

    protected override void OnEnable()
    {
        
    }


    protected override void Update()
    {
        base.Update();
    }





    //set the target and move to it
    public void GoToTarget(Transform destination)
    {
        destinationTransform = destination;
        destinationSetter.target = destinationTransform;
    }
}


