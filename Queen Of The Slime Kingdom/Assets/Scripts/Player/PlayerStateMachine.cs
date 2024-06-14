using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    // States
    public PlayerMovingState MovingState { get; }
    public PlayerAttackingState AttackingState { get; }

    public float MoveSpeed { get; }
    public float AttackInterval { get; }

    public PlayerStateMachine(Player player, float moveSpeed, float attackInterval)
    {
        this.Player = player;

        MoveSpeed = moveSpeed;
        AttackInterval = attackInterval;

        MovingState = new PlayerMovingState(this);
        AttackingState = new PlayerAttackingState(this);
    }
}

