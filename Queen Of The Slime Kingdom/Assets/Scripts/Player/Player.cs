using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackInterval = 2f;
    public float pushBackForce = 5f; // 플레이어를 밀어내는 힘
    public int maxCollisionCount = 3; // 최대 충돌 횟수

    private PlayerStateMachine stateMachine;
    private Rigidbody rb;
    private int collisionCount; // 충돌 횟수 추적 변수

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this, moveSpeed, attackInterval);
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.MovingState);
    }

    // 충돌이 아닌, Raycast로 몬스터를 감지했을 때, 공격 메서드 실행
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Monster"))
        {
            collisionCount++; // 충돌 횟수 증가

            if (collisionCount >= maxCollisionCount)
            {
                Debug.Log($"몬스터 사망");
                // 몬스터 풀로 반환
                GameManager.Instance.ObjectPool.ReturnToPool(collision.gameObject);
                collisionCount = 0; // 충돌 횟수 초기화
                stateMachine.ChangeState(stateMachine.MovingState);
                
            }
            else
            {
                stateMachine.ChangeState(stateMachine.AttackingState);
                Debug.Log($"{collisionCount}/{maxCollisionCount}");
                StartCoroutine(PushBackCoroutine());
            }
        }
    }

    private IEnumerator PushBackCoroutine()
    {
        Vector3 pushDirection = new Vector3(0, 0, -1);
        rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);

        // 일정 시간 동안 대기 (밀려나는 동작이 완료될 때까지)
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        // 밀려난 후 이동 상태로 전환
        stateMachine.ChangeState(stateMachine.MovingState);
    }
}

