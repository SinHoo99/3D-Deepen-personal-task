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

        PoolDictionary = new Dictionary<string, Queue<GameObject>>(); // ��ü Ǯ ���� �ʱ�ȭ
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                // Health ������Ʈ�� ���� ��� Respawn �޼��带 ȣ���Ͽ� IsDie�� �ʱ�ȭ
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

        // �߰� �ʱ�ȭ (��: ��ġ ����)
        objToSpawn.transform.position = position;

        // Health ������Ʈ�� ���� ��� Respawn �޼��带 ȣ���Ͽ� IsDie�� �ʱ�ȭ
        Health healthComponent = objToSpawn.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.Respawn();
        }

        // �ʿ��� �ʱ�ȭ �۾� �߰� ����

        objToSpawn.SetActive(true);

        // ��� �� �ٽ� Ǯ�� �ֱ�
        PoolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
