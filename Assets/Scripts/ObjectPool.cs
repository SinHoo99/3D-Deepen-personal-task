using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        PoolDictionary = new Dictionary<string, Queue<GameObject>>(); // 객체 풀 사전 초기화
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                // Health 컴포넌트가 있을 경우 Respawn 메서드를 호출하여 IsDie를 초기화
                Health healthComponent = obj.GetComponent<Health>();
                if (healthComponent != null)
                {
                    healthComponent.Respawn();
                }
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objToSpawn = PoolDictionary[tag].Dequeue();

        // 추가 초기화 (예: 위치 설정)
        objToSpawn.transform.position = position;

        // Health 컴포넌트가 있을 경우 Respawn 메서드를 호출하여 IsDie를 초기화
        Health healthComponent = objToSpawn.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.Respawn();
        }

        // 필요한 초기화 작업 추가 가능

        objToSpawn.SetActive(true);

        // 사용 후 다시 풀에 넣기
        PoolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
