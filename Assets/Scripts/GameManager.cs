using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ObjectPool ObjectPool { get; private set; }
    public float spawnInterval = 5f; // ���� �����ϴ� ���� (��)
    public GameObject player; // �÷��̾� GameObject ����

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ObjectPool ������Ʈ�� ã�Ƽ� ObjectPool�� �Ҵ�
        ObjectPool = FindObjectOfType<ObjectPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("ObjectPool ������Ʈ�� ã�� ���߽��ϴ�.");
        }
    }

    private void Start()
    {
        SpawnInitialEnemy();
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void SpawnInitialEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject initialEnemy = ObjectPool.SpawnFromPool("Enemy", spawnPosition);
        if (initialEnemy != null)
        {
            Debug.Log("�ʱ� �� ���� �Ϸ�");
        }
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject newEnemy = ObjectPool.SpawnFromPool("Enemy", spawnPosition);
            if (newEnemy != null)
            {
                Debug.Log("�߰� �� ���� �Ϸ�");
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
            return Vector3.zero;
        }

        Vector3 playerPosition = player.transform.position;

        // �÷��̾��� ��ġ���� ���� ���� ������ ������ ��ġ ���
        float randomXOffset = Random.Range(-100f, 100f);
        float randomZOffset = Random.Range(-100f, 100f);

        // ������ ��ġ ���
        Vector3 spawnPosition = new Vector3(
            playerPosition.x + randomXOffset,
            0.5f, // Y�� ����
            playerPosition.z + randomZOffset
        );

        return spawnPosition;
    }
}
