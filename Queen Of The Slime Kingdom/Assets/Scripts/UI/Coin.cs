using System.Collections;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // MoneyPanel의 텍스트
    public int coinValue = 100; // 동전 가치
    public GameObject coin;
    public bool isCoinReady;

    private Store store;

    private void Start()
    {
        store = FindObjectOfType<Store>();
    }

    private void Update()
    {
        if (isCoinReady)
        {
            ActivateCoin();
        }
    }

    public void ActivateCoin()
    {
        coin.SetActive(true);
        StartCoroutine(DeactivateCoinAfterTime(1f)); // 1초 후에 비활성화
        isCoinReady = false;
    }

    private IEnumerator DeactivateCoinAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        coin.SetActive(false);
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (int.TryParse(moneyText.text, out int currentMoney))
        {
            currentMoney += coinValue;
            moneyText.text = currentMoney.ToString();
        }
    }
}
