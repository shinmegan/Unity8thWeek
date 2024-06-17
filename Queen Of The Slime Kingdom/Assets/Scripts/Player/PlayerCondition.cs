using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable // 데이지를 받을 수 있는 오브젝트 인터페이스
{
    void TakeDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("UI Condition")]
    public UICondition uiCondition;
    private Condition hp { get { return uiCondition.hp; } }
    private Condition mp { get { return uiCondition.mp; } }

    public event Action onTakeDamage;
    public event Action OnDeath;

    private bool isDead;

    private void Update()
    {
        if(hp.curValue == 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        hp.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("플레이어가 죽었다.");
    }

    // 최대 체력 설정 메서드
    public void SetMaxHealth(float maxHp)
    {
        hp.SetMaxValue(maxHp);
        hp.curValue = maxHp; // 최대 체력 설정 후 현재 체력도 초기화
    }

}
