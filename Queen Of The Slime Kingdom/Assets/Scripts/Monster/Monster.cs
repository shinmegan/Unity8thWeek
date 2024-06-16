using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    [SerializeField] private string monsterTag = "Monster";
    [SerializeField] private float initialZ = 25.08f;
    [SerializeField] private float minZIncrement = 10f;
    [SerializeField] private float maxZIncrement = 35f;
    public int currentStage = 1; // 현재 스테이지 정보
    public MonsterStats stats;
    public int currentHealth; // 현재 체력

    private float currentZ;
    private float timeSinceLastSpawn;
    private GameManager gameManager;
    private ObjectPool objectPool;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectPool = gameManager.ObjectPool;
        currentZ = initialZ;
        timeSinceLastSpawn = 0f;

        InitializeMonster();

        // 초기 몬스터 생성
        Vector3 initialPosition = new Vector3(17.42f, 2.86f, currentZ);
        SpawnMonster(initialPosition);
    }


    void Update()
    {
        UpdateMonsterSpawn();
    }

    void SpawnMonster(Vector3 spawnPosition)
    {
        GameObject monster = objectPool.SpawnFromPool(monsterTag, spawnPosition, Quaternion.identity);
        InitializeMonster();
    }

    void UpdateNextSpawnPosition()
    {
        float zIncrement = Random.Range(minZIncrement, maxZIncrement);
        float posX = 17.42f;
        currentZ += zIncrement;
        Vector3 nextPosition = new Vector3(posX, 2.86f, currentZ);
        SpawnMonster(nextPosition);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public void InitializeMonster()
    {
        currentHealth = stats.defaltsMaxHP;
    }

    private void UpdateMonsterSpawn()
    {
        // 몬스터 풀에서 비활성화된 몬스터가 있는지 확인
        var monsterQueue = GameManager.Instance.ObjectPool.PoolDictionary[monsterTag];
        var monsterArray = monsterQueue.ToArray();
        foreach (GameObject monster in monsterArray)
        {
            if (!monster.activeInHierarchy)
            {
                UpdateNextSpawnPosition();
            }
        }
    }
}
