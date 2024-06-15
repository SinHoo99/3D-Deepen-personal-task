using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChasingState : PlayerBaseState
{
    public PlayerChasingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        // ���� ������ ���� ����
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        UpdateTrackedEnemies(); // ó���� ������ ������ ������Ʈ
        UpdateTarget(); // ���� ����� ���� ã�� Target���� ����
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

        // ���� Ÿ���� null�̰ų� ���� ��� �ٸ� ���� ������
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // ���� ����� ���� ã�� Target���� ����
        }

        // ���� ���� ������ ���� ��� Attack ���·� ��ȯ
        if (IsInAttackRange())
        {
            UpdateTarget(); // ���� ������ ������ ���� ������
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else if(!IsInAttackRange()) 
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }

    // ������ ������ ������Ʈ�ϴ� �޼���
    private void UpdateTrackedEnemies()
    {
        stateMachine.EnemyTracker.UpdateTrackedEnemies();
    }

    // ���� ����� ���� ������Ʈ�ϴ� �޼���
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
    public bool IsInAttackRange()
    {
        if (stateMachine.Target == null)
            return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
        
    }

}
