using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public Slider gaugeSlider;

    public PlayerStats stats;

    private void Start()
    {
        curValue = startValue;
        maxValue = stats.maxHp;
    }

    private void Update()
    {
        gaugeSlider.value = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    // 최대 체력 업데이트 메서드
    public void SetMaxValue(float value)
    {
        maxValue = value;
    }
}
