using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // MoneyPanel의 텍스트
    public Transform moneyPanelTransform; // MoneyPanel의 Transform
    public int coinValue = 10; // 동전 가치

    public void SpawnAndAnimateCoins(Vector3 spawnPosition, int coinCount)
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f; // 주위에 뿌릴 위치 계산
            randomOffset.y = 0; // y축 이동 방지
            Vector3 coinPosition = spawnPosition + randomOffset;
            GameObject coin = GameManager.Instance.ObjectPool.SpawnFromPool("Coin", coinPosition, Quaternion.identity);
            StartCoroutine(AnimateCoin(coin));
        }
    }

    private IEnumerator AnimateCoin(GameObject coin)
    {
        Vector3 startPosition = coin.transform.position;
        Vector3 endPosition = moneyPanelTransform.position;
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            coin.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coin.transform.position = endPosition;
        GameManager.Instance.ObjectPool.ReturnToPool(coin);
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        int currentMoney = int.Parse(moneyText.text);
        currentMoney += coinValue;
        moneyText.text = currentMoney.ToString();
    }
}
