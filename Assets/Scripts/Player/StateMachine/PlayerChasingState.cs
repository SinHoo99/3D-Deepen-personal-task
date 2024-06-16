using UnityEngine;

public class PlayerChasingState : PlayerBaseState
{
    public PlayerChasingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        UpdateTrackedEnemies();
        UpdateTarget(); // ���� ��� ������Ʈ

        stateMachine.EnemyTracker.DebugTrackedEnemies();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // ���� ����� ���ų� �׾����� ���ο� ��� ã��
        }

        if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState); // ���� ���� ���� ������ ���� ���·� ��ȯ
        }
        else if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.GroundState); // ���� ���� �ۿ� ������ �⺻ ���·� ��ȯ
        }
    }

    private void UpdateTrackedEnemies()
    {
        stateMachine.EnemyTracker.UpdateTrackedEnemies(); // �� ���� ������Ʈ
    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();

        float closestDistanceSqr = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (var enemy in stateMachine.EnemyTracker.GetAllTrackedEnemies())
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

    public bool IsInAttackRange()
    {
        if (stateMachine.Target == null)
            return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
    }
}
