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

        PoolDictionary = new Dictionary<string, Queue<GameObject>>(); // �ʱ�ȭ�� �ʿ���
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
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

        GameObject obj = PoolDictionary[tag].Dequeue();
        obj.transform.position = position; // ��ġ ����
        obj.SetActive(true);

        // ��Ÿ �ʱ�ȭ �ڵ� �߰�

        PoolDictionary[tag].Enqueue(obj);
        return obj;
    }

}
