using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    [SerializeField] private string monsterTag = "Monster";
    [SerializeField] private float initialZ = 25.08f;
    [SerializeField] private float minZIncrement = 10f;
    [SerializeField] private float maxZIncrement = 15f;
    public int currentStage = 1; // 현재 스테이지 정보
    public MonsterStats stats;
    public int currentHealth; // 현재 체력

    public List<GameObject> activeMonsters = new List<GameObject>();
    private float currentZ;
    private GameManager gameManager;
    public ObjectPool objectPool;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectPool = gameManager.ObjectPool;
        currentZ = initialZ;

        // 초기 몬스터 생성
        for (int i = 0; i < objectPool.PoolDictionary[monsterTag].Count; i++)
        {
            float zIncrement = Random.Range(minZIncrement, maxZIncrement);
            Vector3 initialPosition = new Vector3(17.42f, 2.86f, currentZ + i * zIncrement);
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
        float posX = 17.42f;
        currentZ = maxZ + zIncrement;
        Vector3 nextPosition = new Vector3(posX, 2.86f, currentZ);
        SpawnMonster(nextPosition);
    }
}
