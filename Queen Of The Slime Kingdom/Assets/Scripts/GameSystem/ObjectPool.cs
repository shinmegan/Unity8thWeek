using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag; // 태그
        public GameObject prefab; // 프리팹
        public int size; // 크기
    }

    public List<Pool> Pools; 
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    public void Initialize()
    {
        // 오브젝트 풀 생성
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // 비활성화
                obj.transform.SetParent(transform); // ObjectPool의 자식으로 설정
                objectPool.Enqueue(obj); // 큐에 추가
            }
            PoolDictionary.Add(pool.tag, objectPool); // 딕셔너리에 추가
        }
    }

    // 풀 생성 메서드
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // 해당 태그의 풀 존재 확인
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} does not exist.");
            return null;
        }

        // 오브젝트 풀에서 오브젝트 가져오기
        GameObject obj = PoolDictionary[tag].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true); // 활성화

        // 오브젝트 초기화 호출
        var monsterComponent = obj.GetComponent<Monster>();
        if (monsterComponent != null)
        {
            monsterComponent.InitializeMonster();
        }

        return obj;
    }

    // 풀로 반환 메서드
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        // 오브젝트의 태그를 기준으로 딕셔너리에서 찾아서 넣어주기
        string objTag = obj.tag;
        if (PoolDictionary.ContainsKey(objTag))
        {
            PoolDictionary[objTag].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Object with tag {objTag} cannot be returned to pool");
        }
    }

    // 비활성화된 오브젝트가 있는지 확인하는 메서드
    public bool HasInactiveObject(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} does not exist.");
            return false;
        }

        foreach (var obj in PoolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }
}
