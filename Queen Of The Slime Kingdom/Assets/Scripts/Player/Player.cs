using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Info")]
    public float moveSpeed = 5f;
    public float attackInterval = 2f;
    public float pushBackForce = 5f; // 플레이어를 밀어내는 힘
    public float detectionRadius = 11f;
    public float detectionAngle = 60f;
    public int attackPower; // 공격력

    [Header("UI")]
    public Coin coin; // 코인 재화
    public TextMeshProUGUI damageTextInstance;

    [Header("Class")]
    public PlayerStats stats;
    public PlayerCondition condition;
    private PlayerStateMachine stateMachine;
    private Monster monster;
    private Rigidbody rb;

    [Header("EXP")]
    public int currentExperience;
    public int maxExperience = 1000;
    public int getExpAmount = 100;
    public int level; // 플레이어 레벨
    public Slider experienceSlider; // 경험치 바 Slider
    public TextMeshProUGUI levelText; // 레벨 텍스트 Text

    private int initialMaxHp; // 초기 최대 체력 저장

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        condition = GetComponent<PlayerCondition>();

        stateMachine = new PlayerStateMachine(this, moveSpeed, attackInterval);
        rb = GetComponent<Rigidbody>();
        attackPower = stats.attackPower;
        currentExperience = 0;
        level = 1;

        // 초기 최대 체력 저장 및 설정
        initialMaxHp = stats.startHp;
        condition.SetMaxHealth(initialMaxHp);

        UpdateExperienceUI();
        UpdateLevelUI();
    }

    private void Start()
    {
        InitializePlayerStats(); // 게임 시작 시 초기화 메서드 호출
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
                coin.isCoinReady = true; // 동전 이미지표시 및 획득
                GameManager.Instance.GainExperience(this,getExpAmount); // 몬스터를 물리쳐서 100 경험치 획득
                // 몬스터 풀로 반환
                GameManager.Instance.ObjectPool.ReturnToPool(collision.gameObject);
                monster.activeMonsters.Remove(collision.gameObject);
                monster.UpdateMonsterSpawn();
                stateMachine.ChangeState(stateMachine.MovingState);

                // 레벨업 체크
                GameManager.Instance.CheckLevelUp(this);
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
        // 데미지 텍스트 표시
        damageTextInstance.text = "- " + damageAmount.ToString();
        damageTextInstance.gameObject.SetActive(true);

        // 일정 시간 후에 텍스트 제거
        StartCoroutine(OffDamageText(damageTextInstance));
    }

    private IEnumerator OffDamageText(TextMeshProUGUI damageTextInstance)
    {
        yield return new WaitForSeconds(0.5f); 
       damageTextInstance.gameObject.SetActive(false);
    }

    // UI 업데이트 메서드들은 GameManager에서 호출
    public void UpdateExperienceUI()
    {
        float experienceRatio = (float)currentExperience / maxExperience;
        experienceSlider.value = experienceRatio;
    }

    // 레벨 업 UI 업데이트 메서드
    public void UpdateLevelUI()
    {
        levelText.text = $"Lv\n{level}";
    }
    // 레벨업 시 호출되는 메서드
    public void OnLevelUp()
    {
        stats.IncreaseMaxHpOnLevelUp(); // 최대 체력 증가
        condition.SetMaxHealth(stats.maxHp); // Condition의 maxValue 업데이트 및 현재 체력 회복
        Debug.Log($"레벨 업! 새로운 최대 체력: {stats.maxHp}");
        if (monster != null)
        {
            monster.UpdateAttackPower(); // 레벨업 시 몬스터 공격력 업데이트
        }
    }

    // 게임 시작 시 초기화 메서드
    public void InitializePlayerStats()
    {
        stats.maxHp = initialMaxHp; // 초기 최대 체력으로 재설정
        condition.SetMaxHealth(initialMaxHp); // Condition의 maxValue 업데이트 및 현재 체력 회복
        currentExperience = 0;
        level = 1;
        UpdateExperienceUI();
        UpdateLevelUI();
    }
}

