using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hp;
    public Condition mp;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
