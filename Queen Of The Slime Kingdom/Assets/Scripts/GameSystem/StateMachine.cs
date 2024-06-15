using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태 인터페이스
public interface IState
{
    void Enter();
    void Exit();
}

// 상태머신 추상클래스
public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}

