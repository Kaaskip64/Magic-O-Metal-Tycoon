using Pathfinding;
using System.Collections;
using UnityEngine;

public class Guest : NPC_FSM
{
    [Header("References")]
    public AIDestinationSetter destinationSetter;
    public AIPath aIPath;
    public IdleState idleState;
    public BreakState breakState;
    public CheerState cheerState;
    public RestoreState restoreState;
    public TestState testState;

    [Header("NPC Stats")]
    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;

    [Header("Movement Parameters")]
    public int numberOfRays = 5; // 射线数量
    public float rayLength = 2f; // 射线长度
    public float angleRange = 120f; // 射线范围（度）
    public float offsetRatio = 5;

    private Transform destinationTransform;
    private Rigidbody2D rb;
    private Vector2 movingDirection;

    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        idleState = new IdleState();
        cheerState = new CheerState();
        breakState = new BreakState();
        restoreState = new RestoreState();

        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {

        // 初始化NPC状态值
        hungryMeter = NPCGlobalData.Instance.initialHungryMeter + Random.Range(-10, 10);
        thristMeter = NPCGlobalData.Instance.initialThristMeter + Random.Range(-10, 10);
        urgencyMeter = NPCGlobalData.Instance.initialUregencyMeter + Random.Range(-10, 10);
        satisfaction = NPCGlobalData.Instance.initialSatisfaction + Random.Range(-10, 10);

        // 切换到欢呼状态
        SwitchState(cheerState);
    }

    protected override void Update()
    {
        base.Update();

        // 更新NPC的移动方向
        movingDirection = (destinationTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        // 检测碰撞并避免
        CollisionAvoid();
    }

    // 设置目标并移动向它
    public void GoToTarget(Transform destination)
    {
        destinationTransform = destination;
        destinationSetter.target = destinationTransform;
    }

    private void CollisionAvoid()
    {
        // 获取当前物体的朝向作为射线的方向
        Vector2 direction = movingDirection.normalized;

        // 计算每条射线之间的角度间距
        float angleStep = angleRange / (numberOfRays - 1);

        // 计算第一条射线的起始角度
        float startAngle = -angleRange / 2;

        // 循环发射射线
        for (int i = 0; i < numberOfRays; i++)
        {
            // 计算当前射线的角度
            float angle = startAngle + angleStep * i;

            // 将角度转换为方向向量
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction;

            Vector2 rayPosition = new Vector2(transform.position.x + movingDirection.x * 0.6f, transform.position.y + movingDirection.y * 0.6f);

            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, rayDirection, rayLength);
            Debug.DrawRay(rayPosition, rayDirection * rayLength, Color.red);

            // 如果射线击中了物体
            if (hit.collider != null && hit.collider.CompareTag("NPC") && hit.collider.transform != transform)
            {
                // 在控制台输出击中的物体信息
                Debug.Log("Ray hit: " + hit.collider.gameObject.name + " at distance: " + hit.distance);
                Vector2 perpendicularDirection = new Vector2(-movingDirection.y, movingDirection.x);
                rb.AddForce(perpendicularDirection* offsetRatio);
            }
        }
    }

    protected override void OnEnable()
    {
        
    }

    public void ExecuteCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
