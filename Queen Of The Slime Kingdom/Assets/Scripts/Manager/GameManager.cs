using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform PlayerTransform { get; private set; } // 플레이어
    public ObjectPool ObjectPool { get; private set; } // 오브젝트 풀
    public Coin coin;
    [SerializeField] private string playerTag = "Player";
    public int currentStage = 1; // 현재 스테이지 정보

    private void Awake()
    {
        if (Instance == null)
        {   // 인스턴스 초기화
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
                return;
            }
        }

        PlayerTransform = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        ObjectPool = GetComponent<ObjectPool>();

        // Pool 정보 설정
        ObjectPool.Initialize(); // 오브젝트 풀 초기화
        coin = FindObjectOfType<Coin>(); // 코인 초기화
    }

    public void AdvanceToNextStage()
    {
        currentStage++;
        UpdateMonsterStatsForStage(currentStage);
    }

    private void UpdateMonsterStatsForStage(int stage)
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            //monster.currentStage = stage;
            //var stageStats = monster.stats.GetStatsForStage(stage);
            //monster.stats.defaultHP = stageStats.HP;
            //monster.stats.defaultAttackPower = stageStats.attackPower;
            //monster.stats.defaultAttackSpeed = stageStats.attackSpeed;
            //monster.stats.defaultMoveSpeed = stageStats.moveSpeed;
        }
    }
}
