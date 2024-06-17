using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class StageSpecificStats
{
    public int stage;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int initialAttackPower;
}

[CreateAssetMenu(fileName = "MonsterStats", menuName = "NewMonsterStats", order = 2)]
public class MonsterStats : ScriptableObject
{
    public int defaultHP;
    public int defaltsMaxHP;
    public int defaultAttackPower;
    public int defaultInitialAttackPower;
    public List<StageSpecificStats> stageSpecificStats = new List<StageSpecificStats>();

    public StageSpecificStats GetStatsForStage(int stage)
    {
        if (stageSpecificStats == null || stageSpecificStats.Count == 0)
        {
            return new StageSpecificStats
            {
                currentHealth = defaultHP,
                attackPower = defaultAttackPower,
                maxHealth = defaltsMaxHP,
                initialAttackPower = defaultInitialAttackPower
            };
        }

        foreach (var stats in stageSpecificStats)
        {
            if (stats.stage == stage)
                return stats;
        }

        return new StageSpecificStats
        {
            currentHealth = defaultHP,
            attackPower = defaultAttackPower,
            maxHealth = defaltsMaxHP,
            initialAttackPower = defaultInitialAttackPower
        };
    }

    public void IncreaseMonsterAttack(int level)
    {
        defaultAttackPower = Mathf.RoundToInt((defaultAttackPower * 1.3f) +level); // 기존 공격력 1.3배 + 플레이어 레벨
    }

    public void SetAttackValue(int value)
    {
        defaultAttackPower = value;
    }
}
