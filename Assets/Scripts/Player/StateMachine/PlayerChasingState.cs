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
        UpdateTarget(); // 추적 대상 업데이트

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
            UpdateTarget(); // 추적 대상이 없거나 죽었으면 새로운 대상 찾기
        }

        if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState); // 공격 범위 내에 있으면 공격 상태로 전환
        }
        else if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.GroundState); // 추적 범위 밖에 있으면 기본 상태로 전환
        }
    }

    private void UpdateTrackedEnemies()
    {
        stateMachine.EnemyTracker.UpdateTrackedEnemies(); // 적 추적 업데이트
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
            Debug.Log($"현재 추적 중인 적: {stateMachine.Target.gameObject.name}");
        }
        else
        {
            Debug.Log("현재 추적 중인 적이 없습니다.");
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
