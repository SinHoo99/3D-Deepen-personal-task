using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ObjectPool ObjectPool { get; private set; }
    public float spawnInterval = 5f; // 적을 생성하는 간격 (초)
    public GameObject player; // 플레이어 GameObject 참조

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ObjectPool 컴포넌트를 찾아서 ObjectPool에 할당
        ObjectPool = FindObjectOfType<ObjectPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("ObjectPool 컴포넌트를 찾지 못했습니다.");
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
            Debug.Log("초기 적 생성 완료");
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
                Debug.Log("추가 적 생성 완료");
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
            return Vector3.zero;
        }

        Vector3 playerPosition = player.transform.position;

        // 플레이어의 위치에서 일정 범위 내에서 랜덤한 위치 계산
        float randomXOffset = Random.Range(-100f, 100f);
        float randomZOffset = Random.Range(-100f, 100f);

        // 랜덤한 위치 계산
        Vector3 spawnPosition = new Vector3(
            playerPosition.x + randomXOffset,
            0.5f, // Y축 고정
            playerPosition.z + randomZOffset
        );

        return spawnPosition;
    }
}
