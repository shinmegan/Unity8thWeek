using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    [Header("SerializeField")]
    [SerializeField] private string monsterTag = "Monster";
    [SerializeField] private float initialZ = 25.08f;
    [SerializeField] private float minZIncrement = 20f;
    [SerializeField] private float maxZIncrement = 40f;

    [Header("Info")]
    public MonsterStats stats;
    public int currentStage = 1; // 현재 스테이지 정보
    public int currentHealth; // 현재 체력
    public int currentAttackPower; // 현재 공격력

    public ObjectPool objectPool;
    public List<GameObject> activeMonsters = new List<GameObject>();

    private float currentZ;
    private GameManager gameManager;
    private Player player; // 플레이어 참조 추가
    private int initialAttackPower; // 초기 공격력 저장

    private void Awake()
    {
        // 초기 공격력 저장 및 설정
        initialAttackPower = stats.defaultInitialAttackPower;
        stats.SetAttackValue(initialAttackPower);
        InitializeMonsterStats();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>(); // 플레이어 인스턴스 찾기
        objectPool = gameManager.ObjectPool;
        currentZ = initialZ;

        

        // 초기 몬스터 생성
        for (int i = 0; i < objectPool.PoolDictionary[monsterTag].Count; i++)
        {
            float zIncrement = Random.Range(minZIncrement, maxZIncrement);
            float posX = Random.Range(14f, 18f);
            Vector3 initialPosition = new Vector3(posX, 2.86f, currentZ + i * zIncrement);
            SpawnMonster(initialPosition);
        }
    }

    // 몬스터 생성 메서드
    void SpawnMonster(Vector3 spawnPosition)
    {
        GameObject monster = objectPool.SpawnFromPool(monsterTag, spawnPosition, Quaternion.identity);
        activeMonsters.Add(monster);
        InitializeMonster();
    }

    // 데미지 적용 메서드
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    // 몬스터 체력 초기화 메서드
    public void InitializeMonster()
    {
        currentHealth = stats.defaltsMaxHP;
        currentAttackPower = stats.defaultAttackPower;
    }
    public void UpdateAttackPower()
    {
        if (player != null)
        {
            stats.IncreaseMonsterAttack(player.level);
            Debug.Log($"몬스터 공격력 상승: {stats.defaultAttackPower}");
        }
    }

    public void UpdateMonsterSpawn()
    {
        // 가장 큰 Z 값 찾기
        float maxZ = float.MinValue;
        foreach (var activeMonster in activeMonsters)
        {
            float monsterZ = activeMonster.transform.position.z;
            if (monsterZ > maxZ)
            {
                maxZ = monsterZ;
            }
        }
        // 다음 몬스터 생성 위치 설정
        float zIncrement = Random.Range(minZIncrement, maxZIncrement);
        float posX = Random.Range(14f, 18f);
        currentZ = maxZ + zIncrement;
        Vector3 nextPosition = new Vector3(posX, 2.86f, currentZ);
        SpawnMonster(nextPosition);
    }

    // 게임 시작 시 초기화 메서드
    public void InitializeMonsterStats()
    {
        stats.defaultAttackPower = initialAttackPower;
        stats.SetAttackValue(initialAttackPower);
        UpdateAttackPower();
    }
}
