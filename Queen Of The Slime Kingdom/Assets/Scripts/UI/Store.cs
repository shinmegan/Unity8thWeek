using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject shopPanel; // 상점 패널
    public Button closeButton; // 닫기 버튼
    public Button storeButton; // 상점 버튼
    public TextMeshProUGUI moneyText; // 재화 텍스트(Coin.cs)

    // 아이템 가격
    private int hpPotionPrice = 500;
    private int mpPotionPrice = 500;
    private int powerPotionPrice = 700;
    private int weaponPrice = 2000;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseShop);
        storeButton.onClick.AddListener(OpenShop);
    }
    // 상점 활성화 메서드
    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }
    // 상점 비활성화 메서드
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
    // 코인 스크립트에서 재화 값 가져오는 메서드
    private int GetCurrentMoney()
    {
        if (int.TryParse(moneyText.text.Replace(" G", ""), out int currentMoney))
        {
            return currentMoney;
        }
        return 0;
    }

    private void SetCurrentMoney(int amount)
    {
        moneyText.text = amount.ToString();
    }

    public void BuyHPPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= hpPotionPrice)
        {
            currentMoney -= hpPotionPrice;
            Debug.Log("HP 포션 구매 완료");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    public void BuyMPPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= mpPotionPrice)
        {
            currentMoney -= mpPotionPrice;
            Debug.Log("MP 포션 구매 완료");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    public void BuyPowerPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= powerPotionPrice)
        {
            currentMoney -= powerPotionPrice;
            Debug.Log("검 구매 완료");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    public void BuyWeapon()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= weaponPrice)
        {
            currentMoney -= weaponPrice;
            Debug.Log("총 구매 완료");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }
}
