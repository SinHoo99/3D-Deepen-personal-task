using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private List<Enemy> trackedEnemies; // 추적할 적의 목록

    private void Awake()
    {
        trackedEnemies = new List<Enemy>();
    }

    private void Start()
    {
        UpdateTrackedEnemies();
    }

    private void Update()
    {
        UpdateTrackedEnemies();
    }

    // 추적할 적들을 업데이트하는 메서드
    public void UpdateTrackedEnemies()
    {
        trackedEnemies.Clear();
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null && !enemy.health.IsDie)
            {
                trackedEnemies.Add(enemy);
            }
        }
    }

    // 특정 위치에서 가장 가까운 적을 반환하는 메서드
    public Enemy GetClosestEnemy(Vector3 position)
    {
        if (trackedEnemies.Count == 0)
            return null;

        float closestDistanceSqr = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (var enemy in trackedEnemies)
        {
            if (enemy != null && !enemy.health.IsDie)
            {
                float distanceSqr = (enemy.transform.position - position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    // 모든 추적 중인 적들을 반환하는 메서드
    public List<Enemy> GetAllTrackedEnemies()
    {
        return trackedEnemies;
    }
}
