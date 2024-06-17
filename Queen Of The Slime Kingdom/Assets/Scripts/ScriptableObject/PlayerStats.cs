using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "NewPlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int attackPower;
    public float attackSpeed;
    public int maxHp; // 최대 체력
    public int startHp;
    // 레벨업 시 최대 체력 증가
    public void IncreaseMaxHpOnLevelUp()
    {
        maxHp = Mathf.RoundToInt(maxHp * 1.2f); // 기존 최대 체력의 1.2배로 증가
    }
}