using System.Collections;
using UnityEngine;

public class PlayerChasingState : IState
{
    private PlayerStateMachine stateMachine;
    private Coroutine chaseCoroutine; // Chase 코루틴을 추적하기 위한 변수
    private Transform targetMonster; // 추적할 몬스터의 Transform

    public PlayerChasingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        StartChaseCoroutine();
    }

    public void Exit()
    {
        StopChaseCoroutine();
    }

    private IEnumerator Chase()
    {
        while (true)
        {
            FindTargetMonster();
            if (targetMonster != null)
            {
                Vector3 direction = (targetMonster.position - stateMachine.Player.transform.position).normalized;
                stateMachine.Player.transform.Translate(direction * stateMachine.MoveSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    private void FindTargetMonster()
    {
        Collider[] hitColliders = Physics.OverlapSphere(stateMachine.Player.transform.position, stateMachine.Player.detectionRadius, LayerMask.GetMask("Monster"));
        float closestDistance = float.MaxValue;
        Transform closestMonster = null;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToMonster = hitCollider.transform.position - stateMachine.Player.transform.position;
            float angle = Vector3.Angle(stateMachine.Player.transform.forward, directionToMonster);
            if (angle < stateMachine.Player.detectionAngle / 2)
            {
                float distance = directionToMonster.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMonster = hitCollider.transform;
                }
            }
        }

        targetMonster = closestMonster;
    }

    public void StartChaseCoroutine()
    {
        if (chaseCoroutine == null)
        {
            chaseCoroutine = stateMachine.Player.StartCoroutine(Chase());
        }
    }

    public void StopChaseCoroutine()
    {
        if (chaseCoroutine != null)
        {
            stateMachine.Player.StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }
    }
}
