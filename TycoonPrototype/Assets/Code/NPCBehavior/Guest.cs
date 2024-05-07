using Pathfinding;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Guest : NPC_FSM
{
    [HideInInspector]
    public AIDestinationSetter destinationSetter;
    public IdleState idleState;
    public BreakState breakState;
    public CheerState cheerState;
    public RestoreState restoreState;
    public LeaveParkState leaveParkState;
    [HideInInspector]
    public AIPath aIPath;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    [Header("Movement Parameter")]
    public float maxSpeed;

    [Header("NPC Stats")]
    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;

    [Header("Crowd Simulation Parameter")]
    public int numberOfRays = 5;
    public float rayLength = 2f;
    public float angleRange = 120f;
    public float offsetRatio = 5;

    public Transform destinationTransform { get; private set; }
    private Rigidbody2D rb;
    private Vector2 movingDirection;
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
        idleState = new IdleState();
        cheerState = new CheerState();
        breakState = new BreakState();
        restoreState = new RestoreState();
        leaveParkState = new();

        //Physics
        rb = GetComponent<Rigidbody2D>();

        //Animator
        animator = GetComponent<Animator>();

        //Sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InstanceInit()
    {
        //NPC value init
        hungryMeter = NPCManager.Instance.initialHungryMeter + Random.Range(-10, 10);
        thristMeter = NPCManager.Instance.initialThristMeter + Random.Range(-10, 10);
        urgencyMeter = NPCManager.Instance.initialUregencyMeter + Random.Range(-10, 10);
        satisfaction = NPCManager.Instance.initialSatisfaction + Random.Range(-10, 10);
    }

    protected override void Start()
    {
        InstanceInit();

        StatesSetup();
    }

    private void StatesSetup()
    {
        //Initialize state
        SwitchState(cheerState);
    }

    protected override void Update()
    {
        base.Update();

        CalculateMovingDirection();
        SpriteFlip();

        if (destinationTransform != null && !aIPath.reachedDestination)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        print(currentState.GetType());
    }

    private void SpriteFlip()
    {
        if (destinationTransform != null && destinationTransform.position.x - transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
    private void PathfindingSetting()
    {
        if(aIPath.maxSpeed!=maxSpeed)
        {
            aIPath.maxSpeed = maxSpeed;
        }
    }
    private void CalculateMovingDirection()
    {
        if(destinationTransform != null)
        {
            movingDirection = (destinationTransform.position - transform.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        base.FixUpdate();
        PathfindingSetting();
        CollisionAvoid();   
    }

    public void GoToTarget(Transform destination)
    {
        destinationTransform = destination;
        destinationSetter.target = destinationTransform;
    }

    private void CollisionAvoid()
    {
        Vector2 direction = movingDirection.normalized;

        float angleStep = angleRange / (numberOfRays - 1);

        float startAngle = -angleRange / 2;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + angleStep * i;

            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction;

            Vector2 rayPosition = new Vector2(transform.position.x + movingDirection.x * 0.6f, transform.position.y + movingDirection.y * 0.6f);

            RaycastHit2D hit = Physics2D.Raycast(rayPosition, rayDirection, rayLength);
            //Debug.DrawRay(rayPosition, rayDirection * rayLength, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("NPC") && hit.collider.transform != transform)
            {
                //Debug.Log("Ray hit: " + hit.collider.gameObject.name + " at distance: " + hit.distance);
                Vector2 perpendicularDirection = new Vector2(-movingDirection.y, movingDirection.x);
                rb.AddForce(perpendicularDirection* offsetRatio);
            }
        }
    }
}
