using Pathfinding;
using System.Collections;
using UnityEngine;

public class S_Guest : MonoBehaviour
{
    [Header("References")]
    public AIDestinationSetter destinationSetter;
    public AIPath aIPath;
    public S_BreakState breakState;
    public S_CheerState cheerState;
    public S_RestoreState restoreState;

    protected S_BaseState currentState;

    [Header("NPC Stats")]
    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;

    [Header("Movement Parameters")]
    public int numberOfRays = 5;
    public float rayLength = 2f;
    public float angleRange = 120f;
    public float offsetRatio = 5;

    private Transform destinationTransform;
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

        //Physics
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        //NPC value init
        hungryMeter = NPCManager.Instance.initialHungryMeter + Random.Range(-10, 10);
        thristMeter = NPCManager.Instance.initialThristMeter + Random.Range(-10, 10);
        urgencyMeter = NPCManager.Instance.initialUregencyMeter + Random.Range(-10, 10);
        satisfaction = NPCManager.Instance.initialSatisfaction + Random.Range(-10, 10);

        //Initialize state
        SwitchState(cheerState);
    }

    public void SwitchState(S_BaseState state)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = state;
        currentState.EnterState(this);
    }

    protected void Update()
    {
        currentState.OnUpdate();

        movingDirection = (destinationTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
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
            Debug.DrawRay(rayPosition, rayDirection * rayLength, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("NPC") && hit.collider.transform != transform)
            {
                Debug.Log("Ray hit: " + hit.collider.gameObject.name + " at distance: " + hit.distance);
                Vector2 perpendicularDirection = new Vector2(-movingDirection.y, movingDirection.x);
                rb.AddForce(perpendicularDirection * offsetRatio);
            }
        }
    }


}
