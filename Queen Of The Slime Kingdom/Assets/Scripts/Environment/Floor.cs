using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private string floorTag = "Floor";
    [SerializeField] private float floorLength = 100f; // Plane의 길이와 동일하게 설정
    private GameManager gameManager;
    private ObjectPool objectPool;
    private Transform playerTransform;
    private Camera mainCamera;

    private Queue<GameObject> activeFloors = new Queue<GameObject>();
    private float zSpawnPosition = 0f;
   

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectPool = gameManager.ObjectPool;
        playerTransform = gameManager.PlayerTransform;
        mainCamera = gameManager.PlayerTransform.GetComponentInChildren<Camera>();

        // 초기 바닥 생성
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = new Vector3(15, 1.2f, i * 200); // i * 200으로 간격을 조절
            SpawnFloor(spawnPosition);
        }
    }

    void Update()
    {
        // 카메라의 Z축 위치 계산
        if (mainCamera == null || playerTransform == null)
        {
            // mainCamera 또는 playerTransform이 null인 경우 처리
            return;
        }

        float cameraZ = mainCamera.transform.position.z;

        // 이전 바닥 반환 조건
        if (activeFloors.Count > 0 && activeFloors.Peek().transform.position.z + floorLength < cameraZ)
        {
            ReturnFloor();
        }
    }

    void SpawnFloor(Vector3 spawnPosition)
    {
        GameObject floor = objectPool.SpawnFromPool(floorTag, spawnPosition, Quaternion.identity);
        activeFloors.Enqueue(floor);
    }

    // 바닥 반환 및 새로운 바닥 생성 메서드
    void ReturnFloor()
    {
        // 이전 바닥 반환
        GameObject floor = activeFloors.Dequeue();
        objectPool.ReturnToPool(floor);

        // 가장 큰 Z 값 찾기
        float maxZ = float.MinValue;
        foreach (var activeFloor in activeFloors)
        {
            float floorZ = activeFloor.transform.position.z;
            if (floorZ > maxZ)
            {
                maxZ = floorZ;
            }
        }

        // 다음 바닥 생성 위치 설정
        zSpawnPosition = maxZ + floorLength*2;

        // 새로운 바닥 생성
        Vector3 spawnPosition = new Vector3(15, 1.2f, zSpawnPosition);
        SpawnFloor(spawnPosition);
    }
}
