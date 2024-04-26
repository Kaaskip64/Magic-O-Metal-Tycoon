using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState : BaseState
{
    private Vector3 initialPosition;
    private float wanderRadius = 5f; // ���������뾶
    private float wanderTimer; // ����������ʱ��
    private float minWaitTime = 1f; // ��С�ȴ�ʱ��
    private float maxWaitTime = 2f; // ���ȴ�ʱ��

    private Guest attachedGuest;
    public override void EnterState(object obj)
    {
        Debug.Log("EnterIdleState");

        attachedGuest = (Guest)obj;
        // ��ȡ��ɫ��ʼλ��
        initialPosition = attachedGuest.transform.position;

        // ��ʼ��������ʱ��
        wanderTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    public override void ExitState()
    {

    }

    public override void OnUpdate()
    {
        // ����������ʱ��
        wanderTimer -= Time.deltaTime;
        Wander();

        // ���������ʱ��С�ڵ���0��ִ��������Ϊ
        if (wanderTimer <= 0f)
        {
            
            wanderTimer = Random.Range(minWaitTime, maxWaitTime); // ��������������ʱ��
        }
    }

    private void Wander()
    {
        // �������ƫ����
        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;

        // �����ƫ����Ӧ�õ���ʼλ�ø���
        Vector3 targetPosition = initialPosition + randomOffset;

        // �����ƶ��ٶ�
        float moveSpeed = 5f; // ���Ը�����Ҫ�����ƶ��ٶ�

        // �ƶ���ɫ��Ŀ��λ��
        attachedGuest.transform.position = Vector3.MoveTowards(attachedGuest.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


}
