using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private List<Enemy> trackedEnemies; // ������ ���� ���

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

    // ������ ������ ������Ʈ�ϴ� �޼���
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

    // Ư�� ��ġ���� ���� ����� ���� ��ȯ�ϴ� �޼���
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

    // ��� ���� ���� ������ ��ȯ�ϴ� �޼���
    public List<Enemy> GetAllTrackedEnemies()
    {
        return trackedEnemies;
    }

    // ������: ���� ���� ������ �α׷� ����ϴ� �޼���
    public void DebugTrackedEnemies()
    {
        Debug.Log($"���� ���� ���� ��: {trackedEnemies.Count}");
        foreach (var enemy in trackedEnemies)
        {
            if (enemy != null)
            {
                Debug.Log($"�� �̸�: {enemy.gameObject.name}, ���� ����: {!enemy.health.IsDie}");
            }
        }
    }
}
