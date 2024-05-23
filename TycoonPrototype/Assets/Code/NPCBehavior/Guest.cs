using Pathfinding;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Guest : NPC_FSM
{
    #region Public Members
    [Header("States")]
    public BreakState BreakState;
    public CheerState cheerState;
    public RestoreState restoreState;
    public LeaveParkState leaveParkState;

    [Header("Movement Parameter")]
    public float maxSpeed;

    [Header("NPC Stats")]
    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;
    #endregion

    #region Properties
    private AIDestinationSetter destinationSetter;
    public AIDestinationSetter DestinationSetter
    {
        get { return destinationSetter; }
    }

    private AIPath aIPath;
    public AIPath AIPath
    {
        get { return aIPath; }
    }

    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
    }

    private Rigidbody2D rb;
    #endregion

    #region Awake
    private void Awake()
    {
        RefrencenInit();
    }

    private void RefrencenInit()
    {
        //pathfinding component
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        //Init states
        cheerState = new CheerState();
        BreakState = new BreakState();
        restoreState = new RestoreState();
        leaveParkState = new();

        //Physics
        rb = GetComponent<Rigidbody2D>();

        //Animator
        animator = GetComponent<Animator>();

        //Sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Start
    private void InstanceInit()
    {
        //NPC value init
        hungryMeter = NPCManager.Instance.initialHungryMeter + Random.Range(-10, 10);
        thristMeter = NPCManager.Instance.initialThristMeter + Random.Range(-10, 10);
        urgencyMeter = NPCManager.Instance.initialUregencyMeter + Random.Range(-10, 10);
        satisfaction = NPCManager.Instance.initialSatisfaction + Random.Range(-10, 10);
    }

    private void StatesSetup()
    {
        //Initialize state
        SwitchState(cheerState);
    }

    protected override void Start()
    {
        InstanceInit();

        StatesSetup();

        NPCManager.Instance.RegisterNPC(this);
    }
    #endregion

    #region Update
    protected override void Update()
    {
        base.Update();
        SpriteFlip();
        AnimatorSetter();
    }

    private void AnimatorSetter()
    {
        if (destinationSetter.target != null && !aIPath.reachedDestination)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void SpriteFlip()
    {
        if (destinationSetter.target != null && destinationSetter.target.position.x - transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
    #endregion

    #region FixedUpdate
    private void PathfindingSetting()
    {
        if(aIPath.maxSpeed!=maxSpeed)
        {
            aIPath.maxSpeed = maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        base.FixUpdate();
        PathfindingSetting(); 
    }
    #endregion

    #region Utilities
    public void GoToTarget(Transform destination)
    {
        destinationSetter.target = destination;
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnregisterNPC(this);
    }
    #endregion
}
