using System.Collections;
using System.Threading;
using UnityEngine;

// 이동 상태 클래스
public class PlayerMovingState : IState
{
    private PlayerStateMachine stateMachine;
    private Coroutine moveCoroutine; // Move 코루틴을 추적하기 위한 변수

    public PlayerMovingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        StartMoveCoroutine();
    }

    public void Exit()
    {
        StopMoveCoroutine();
    }

    private IEnumerator Move()
    {
        while (true)
        {
            stateMachine.Player.transform.Translate(Vector3.forward * stateMachine.MoveSpeed * Time.deltaTime);

            // 몬스터 감지 후 ChasingState로 전환
            if (DetectMonster())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                yield break;
            }

            yield return null;
        }
    }

    private bool DetectMonster()
    {
        Collider[] hitColliders = Physics.OverlapSphere(stateMachine.Player.transform.position, stateMachine.Player.detectionRadius, LayerMask.GetMask("Monster"));
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToMonster = hitCollider.transform.position - stateMachine.Player.transform.position;
            float angle = Vector3.Angle(stateMachine.Player.transform.forward, directionToMonster);
            if (angle < stateMachine.Player.detectionAngle / 2)
            {
                return true;
            }
        }
        return false;
    }

    public void StartMoveCoroutine()
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = stateMachine.Player.StartCoroutine(Move());
        }
    }

    public void StopMoveCoroutine()
    {
        if (moveCoroutine != null)
        {
            stateMachine.Player.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
