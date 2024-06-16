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
            yield return null;
        }
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
