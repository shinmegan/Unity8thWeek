using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform PlayerTransform { get; private set; } // 플레이어
    public ObjectPool ObjectPool { get; private set; } // 오브젝트 풀
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {
        if (Instance == null)
        {   // 인스턴스 초기화
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
                return;
            }
        }

        PlayerTransform = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        ObjectPool = GetComponent<ObjectPool>();

        // Pool 정보 설정
        ObjectPool.Initialize(); // 오브젝트 풀 초기화
    }
}
