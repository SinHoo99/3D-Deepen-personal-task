using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChasingState : PlayerBaseState
{
    private List<Enemy> trackedEnemies; // 추적할 적의 목록

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
        UpdateTrackedEnemies(); // 처음에 추적할 적들을 업데이트
        UpdateTarget(); // 가장 가까운 적을 찾아 Target으로 설정
    }

    public override void Update()
    {
        base.Update();

        // 현재 타겟이 null이거나 죽은 경우 다른 적을 재추적
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // 가장 가까운 적을 찾아 Target으로 설정
        }

        // 적이 추적 범위를 벗어난 경우 Idle 상태로 전환
        if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // 적이 공격 범위에 들어온 경우 Attack 상태로 전환
        if (IsInAttackRange())
        {
            UpdateTarget(); // 공격 범위에 들어왔을 때도 재추적
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        // 일정 시간마다 추적할 적들의 목록을 업데이트
        UpdateTrackedEnemies();

    }

    // 추적할 적들을 업데이트하는 메서드
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

    // 가장 가까운 적을 업데이트하는 메서드
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

        // closestEnemy가 null이 아니면 closestEnemy의 health를 Target으로 설정합니다.
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

    // 공격 범위 체크
    private bool IsInAttackRange()
    {
        if (stateMachine.Target == null)
            return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
    }

    // 추적 범위 체크
    private bool IsInChasingRange()
    {
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
            return false;

        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return enemyDistanceSqr <= stateMachine.Player.Data.PlayerChasingRange * stateMachine.Player.Data.PlayerChasingRange;
    }
}
