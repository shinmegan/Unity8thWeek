using System.Collections;
using UnityEngine;

// 공격 상태 클래스
public class PlayerAttackingState : IState
{
    private PlayerStateMachine stateMachine;

    public PlayerAttackingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        stateMachine.Player.StartCoroutine(Attack());
    }

    public void Exit()
    {
        stateMachine.Player.StopCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        Debug.Log("몬스터를 공격합니다!");
        yield return new WaitForSeconds(stateMachine.AttackInterval);
    }
}
