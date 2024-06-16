using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private string monsterTag = "Monster";
    [SerializeField] private float initialZ = 25.08f;
    [SerializeField] private float minZIncrement = 20f;
    [SerializeField] private float maxZIncrement = 35f;
    [SerializeField] private float minX = 13.45f;
    [SerializeField] private float maxX = 18.9f;
    public float spawnInterval = 3f; // 몬스터 생성 간격 (초 단위)

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

        // 초기 몬스터 생성
        Vector3 initialPosition = new Vector3(Random.Range(minX, maxX), 2.86f, currentZ);
        SpawnMonster(initialPosition);
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            UpdateNextSpawnPosition();
            timeSinceLastSpawn = 0f;
        }
        // 몬스터 반환 조건
    }

    void SpawnMonster(Vector3 spawnPosition)
    {
        GameObject monster = objectPool.SpawnFromPool(monsterTag, spawnPosition, Quaternion.identity);
    }

    void UpdateNextSpawnPosition()
    {
        float zIncrement = Random.Range(minZIncrement, maxZIncrement);
        float posX = Random.Range(minX, maxX);
        currentZ += zIncrement;
        Vector3 nextPosition = new Vector3(posX, 2.86f, currentZ);
        SpawnMonster(nextPosition);
    }

    // 몬스터 반환 및 새로운 몬스터 생성 메서드
}
