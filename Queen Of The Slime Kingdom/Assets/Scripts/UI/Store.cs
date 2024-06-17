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
    public TextMeshProUGUI promptText; // 구매 결과를 표시할 프롬프트 텍스트

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
        ClearPrompt();
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
    // 현재 재화 값 텍스트 표시 메서드
    private void SetCurrentMoney(int amount)
    {
        moneyText.text = amount.ToString();
    }
    // 프롬프트 표시 메서드
    private void ShowPrompt(string message)
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
        Invoke("ClearPrompt", 2f); // 2초 후에 프롬프트 숨김
    }
    // 프롬프트 초기화 메서드
    private void ClearPrompt()
    {
        promptText.text = "";
        promptText.gameObject.SetActive(false);
    }

    public void BuyHPPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= hpPotionPrice)
        {
            currentMoney -= hpPotionPrice;
            ShowPrompt("Bought HP Portion!!");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            ShowPrompt("Money is lacking..");
        }
    }

    public void BuyMPPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= mpPotionPrice)
        {
            currentMoney -= mpPotionPrice;
            ShowPrompt("Bought MP Portion!!");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            ShowPrompt("Money is lacking..");
        }
    }

    public void BuyPowerPotion()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= powerPotionPrice)
        {
            currentMoney -= powerPotionPrice;
            ShowPrompt("Bought PowerUp Portion!!");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            ShowPrompt("Money is lacking..");
        }
    }

    public void BuyWeapon()
    {
        int currentMoney = GetCurrentMoney();
        if (currentMoney >= weaponPrice)
        {
            currentMoney -= weaponPrice;
            ShowPrompt("Bought Weapon!!");
            SetCurrentMoney(currentMoney);
        }
        else
        {
            ShowPrompt("Money is lacking..");
        }
    }
}
