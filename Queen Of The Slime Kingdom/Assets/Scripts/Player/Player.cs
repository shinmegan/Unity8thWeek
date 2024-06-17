using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackInterval = 2f;
    public float pushBackForce = 5f; // 플레이어를 밀어내는 힘
    public int maxCollisionCount = 3; // 최대 충돌 횟수
    public float detectionRadius = 11f;
    public float detectionAngle = 60f;
    public int attackPower; // 공격력

    public PlayerStats stats;
    public PlayerCondition condition;
    private PlayerStateMachine stateMachine;
    private Monster monster;
    private Rigidbody rb;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        condition = GetComponent<PlayerCondition>();
        stateMachine = new PlayerStateMachine(this, moveSpeed, attackInterval);
        rb = GetComponent<Rigidbody>();
        attackPower = stats.attackPower;
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.MovingState);
        monster = FindObjectOfType<Monster>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            if (monster.currentHealth <= 0)
            {
                Debug.Log($"몬스터 사망");
                // 몬스터 풀로 반환
                GameManager.Instance.ObjectPool.ReturnToPool(collision.gameObject);
                monster.activeMonsters.Remove(collision.gameObject);
                monster.UpdateMonsterSpawn();
                stateMachine.ChangeState(stateMachine.MovingState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.AttackingState);
                monster.TakeDamage(attackPower);
                Debug.Log($"몬스터 남은 체력: {monster.currentHealth}");
                StartCoroutine(PushBackCoroutine());
            }
        }
    }

    private IEnumerator PushBackCoroutine()
    {
        Vector3 pushDirection = new Vector3(0, 0, -1f);
        rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);

        // 일정 시간 동안 대기 (밀려나는 동작이 완료될 때까지)
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        // 밀려난 후 이동 상태로 전환
        stateMachine.ChangeState(stateMachine.MovingState);
    }

}

