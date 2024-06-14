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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Monster"))
        {
            Debug.Log("Collision with Monster"); // 몬스터와 충돌 감지 로그
            collisionCount++; // 충돌 횟수 증가

            if (collisionCount >= maxCollisionCount)
            {
                collisionCount = 0; // 충돌 횟수 초기화
                Debug.Log($"몬스터 사망");
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
        Debug.Log("PushBack");
        Vector3 pushDirection = new Vector3(0, 0, -1);
        rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);

        // 일정 시간 동안 대기 (밀려나는 동작이 완료될 때까지)
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        // 밀려난 후 이동 상태로 전환
        stateMachine.ChangeState(stateMachine.MovingState);
    }
}

