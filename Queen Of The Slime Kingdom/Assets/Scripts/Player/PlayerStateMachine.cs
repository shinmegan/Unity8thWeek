using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Animator Animator { get; }

    // States
    public PlayerMovingState MovingState { get; }
    public PlayerAttackingState AttackingState { get; }
    public PlayerChasingState ChasingState { get; }

    public float MoveSpeed { get; }
    public float AttackInterval { get; }

    public PlayerStateMachine(Player player, Animator animator, float moveSpeed, float attackInterval)
    {
        this.Player = player;
        this.Animator = animator;

        MoveSpeed = moveSpeed;
        AttackInterval = attackInterval;

        MovingState = new PlayerMovingState(this);
        AttackingState = new PlayerAttackingState(this, animator);
        ChasingState = new PlayerChasingState(this);
    }
}

