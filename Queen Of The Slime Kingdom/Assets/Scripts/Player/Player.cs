using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackInterval = 2f;
    public float pushBackForce = 5f; // 플레이어를 밀어내는 힘
    public int maxCollisionCount = 3; // 최대 충돌 횟수
    public float detectionRadius = 11f;
    public float detectionAngle = 60f;
    public int attackPower; // 공격력
    public TextMeshProUGUI damageTextPrefab; // 데미지 텍스트 프리팹
    public Transform healthBarTransform; // 체력 바의 Transform
    public Coin coin; // 코인 재화

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
                coin.isCoinReady = true; // 동전 10개 생성 및 애니메이션 실행
                // 몬스터 풀로 반환
                GameManager.Instance.ObjectPool.ReturnToPool(collision.gameObject);
                monster.activeMonsters.Remove(collision.gameObject);
                monster.UpdateMonsterSpawn();
                stateMachine.ChangeState(stateMachine.MovingState);
            }
            else
            {
                condition.TakeDamage(monster.stats.defaultAttackPower); // 몬스터 공격
                ShowDamageText(monster.stats.defaultAttackPower); // 데미지 텍스트 표시
                stateMachine.ChangeState(stateMachine.AttackingState);
                monster.TakeDamage(attackPower);
                Debug.Log($"몬스터 남은 체력: {monster.currentHealth}");
                StartCoroutine(PushBackCoroutine());
            }
        }
    }

    private IEnumerator PushBackCoroutine()
    {
        Vector3 pushDirection = new Vector3(0, 0, -1.5f);
        rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);

        // 일정 시간 동안 대기 (밀려나는 동작이 완료될 때까지)
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        // 밀려난 후 이동 상태로 전환
        stateMachine.ChangeState(stateMachine.MovingState);
    }

    private void ShowDamageText(int damageAmount)
    {
        // 데미지 텍스트 인스턴스화 및 설정
        Vector3 textPosition = healthBarTransform.position;
        textPosition.y += 110; // Top에서 110 위치로 설정
        TextMeshProUGUI damageTextInstance = Instantiate(damageTextPrefab, textPosition, Quaternion.identity, healthBarTransform);
        damageTextInstance.text = "- " + damageAmount.ToString();
        damageTextInstance.gameObject.SetActive(true);

        // 일정 시간 후에 텍스트 제거
        StartCoroutine(RemoveDamageText(damageTextInstance));
    }

    private IEnumerator RemoveDamageText(TextMeshProUGUI damageTextInstance)
    {
        yield return new WaitForSeconds(0.5f); 
        Destroy(damageTextInstance.gameObject);
    }

}

