using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChasingState : PlayerBaseState
{
    private List<Enemy> trackedEnemies; // ������ ���� ���

    public PlayerChasingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        trackedEnemies = new List<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        UpdateTrackedEnemies(); // ó���� ������ ������ ������Ʈ
        UpdateTarget(); // ���� ����� ���� ã�� Target���� ����
    }

    public override void Update()
    {
        base.Update();

        // ���� Ÿ���� null�̰ų� ���� ��� �ٸ� ���� ������
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // ���� ����� ���� ã�� Target���� ����
        }

        // ���� ���� ������ ��� ��� Idle ���·� ��ȯ
        if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // ���� ���� ������ ���� ��� Attack ���·� ��ȯ
        if (IsInAttackRange())
        {
            UpdateTarget(); // ���� ������ ������ ���� ������
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        // ���� �ð����� ������ ������ ����� ������Ʈ
        UpdateTrackedEnemies();

    }

    // ������ ������ ������Ʈ�ϴ� �޼���
    private void UpdateTrackedEnemies()
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

    // ���� ����� ���� ������Ʈ�ϴ� �޼���
    protected override void UpdateTarget()
    {
        float closestDistanceSqr = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (var enemy in trackedEnemies)
        {
            if (enemy != null && !enemy.health.IsDie)
            {
                float distanceSqr = (enemy.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
        }

        // closestEnemy�� null�� �ƴϸ� closestEnemy�� health�� Target���� �����մϴ�.
        stateMachine.Target = closestEnemy != null ? closestEnemy.health : null;
        if (stateMachine.Target != null)
        {
            Debug.Log($"���� ���� ���� ��: {stateMachine.Target.gameObject.name}");
        }
        else
        {
            Debug.Log("���� ���� ���� ���� �����ϴ�.");
        }
    }

    // ���� ���� üũ
    private bool IsInAttackRange()
    {
        if (stateMachine.Target == null)
            return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
    }

    // ���� ���� üũ
    private bool IsInChasingRange()
    {
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
            return false;

        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return enemyDistanceSqr <= stateMachine.Player.Data.PlayerChasingRange * stateMachine.Player.Data.PlayerChasingRange;
    }
}
