using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "NewPlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int attackPower;
    public float attackSpeed;
}

