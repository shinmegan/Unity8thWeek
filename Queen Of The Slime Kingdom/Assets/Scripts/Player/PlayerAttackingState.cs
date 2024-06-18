using System.Collections;
using UnityEngine;

// 공격 상태 클래스
public class PlayerAttackingState : IState
{
    private PlayerStateMachine stateMachine;
    private Animator animator;

    public PlayerAttackingState(PlayerStateMachine stateMachine, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
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
        // Slash 애니메이션 트리거 설정
        animator.SetTrigger("Slash");
        yield return new WaitForSeconds(stateMachine.AttackInterval);
    }
}
