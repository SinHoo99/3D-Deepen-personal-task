using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChasingState : PlayerBaseState
{
    public PlayerChasingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        // 기존 생성자 내용 유지
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        UpdateTrackedEnemies(); // 처음에 추적할 적들을 업데이트
        UpdateTarget(); // 가장 가까운 적을 찾아 Target으로 설정
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

        // 현재 타겟이 null이거나 죽은 경우 다른 적을 재추적
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // 가장 가까운 적을 찾아 Target으로 설정
        }

        // 적이 공격 범위에 들어온 경우 Attack 상태로 전환
        if (IsInAttackRange())
        {
            UpdateTarget(); // 공격 범위에 들어왔을 때도 재추적
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else if(!IsInAttackRange()) 
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }

    // 추적할 적들을 업데이트하는 메서드
    private void UpdateTrackedEnemies()
    {
        stateMachine.EnemyTracker.UpdateTrackedEnemies();
    }

    // 가장 가까운 적을 업데이트하는 메서드
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
    public bool IsInAttackRange()
    {
        if (stateMachine.Target == null)
            return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
        
    }

}
