using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState : BaseState
{
    private Vector3 initialPosition;
    private float wanderRadius = 5f; // 定义漫步半径
    private float wanderTimer; // 定义漫步计时器
    private float minWaitTime = 1f; // 最小等待时间
    private float maxWaitTime = 2f; // 最大等待时间

    private Guest attachedGuest;
    public override void EnterState(object obj)
    {
        Debug.Log("EnterIdleState");

        attachedGuest = (Guest)obj;
        // 获取角色初始位置
        initialPosition = attachedGuest.transform.position;

        // 初始化漫步计时器
        wanderTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    public override void ExitState()
    {

    }

    public override void OnUpdate()
    {
        // 更新漫步计时器
        wanderTimer -= Time.deltaTime;
        Wander();

        // 如果漫步计时器小于等于0，执行漫步行为
        if (wanderTimer <= 0f)
        {
            
            wanderTimer = Random.Range(minWaitTime, maxWaitTime); // 重新设置漫步计时器
        }
    }

    private void Wander()
    {
        // 生成随机偏移量
        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;

        // 将随机偏移量应用到初始位置附近
        Vector3 targetPosition = initialPosition + randomOffset;

        // 计算移动速度
        float moveSpeed = 5f; // 可以根据需要调整移动速度

        // 移动角色向目标位置
        attachedGuest.transform.position = Vector3.MoveTowards(attachedGuest.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


}
