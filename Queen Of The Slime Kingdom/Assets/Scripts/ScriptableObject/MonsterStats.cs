using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageSpecificStats
{
    public int stage;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
}

[CreateAssetMenu(fileName = "MonsterStats", menuName = "NewMonsterStats", order = 2)]
public class MonsterStats : ScriptableObject
{
    public int defaultHP;
    public int defaltsMaxHP;
    public int defaultAttackPower;
    public List<StageSpecificStats> stageSpecificStats = new List<StageSpecificStats>();

    public StageSpecificStats GetStatsForStage(int stage)
    {
        if (stageSpecificStats == null || stageSpecificStats.Count == 0)
        {
            return new StageSpecificStats
            {
                currentHealth = defaultHP,
                attackPower = defaultAttackPower,
                maxHealth = defaltsMaxHP
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
            maxHealth = defaltsMaxHP
        };
    }
}
