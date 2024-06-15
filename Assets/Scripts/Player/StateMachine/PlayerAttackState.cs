using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    bool alreadyAppliedDealing;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.Player.Animator == null)
        {
            Debug.LogError("Animator가 설정되지 않았습니다.");
            return;
        }

        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.BaseAttackParameterName);

        alreadyAppliedDealing = false;

        stateMachine.MovementSpeedModifier = 0;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.BaseAttackParameterName);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.Animator == null)
        {
            Debug.LogError("Animator가 설정되지 않았습니다.");
            return;
        }

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        Debug.Log($"Normalized Time: {normalizedTime}");

        if (normalizedTime < 1f)
        {
            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_Start_TransitionTime)
            {
                Debug.Log("공격 실행");
                stateMachine.Player.Weapon.SetAttack(stateMachine.Player.Data.Damage);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }

            // 적이 사라지면 ChasingState로 전환
            if (stateMachine.Target == null || stateMachine.Target.IsDie || !stateMachine.ChasingState.IsInAttackRange())
            {
                Debug.Log("적이 사라졌습니다. ChasingState로 전환합니다.");
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
        }
        else // normalizedTime이 1 이상이면 공격이 끝난 상태
        {
            if (stateMachine.ChasingState.IsInAttackRange())
            {
                Debug.Log("공격 범위 안에 적이 있습니다. AttackState를 유지합니다.");
                return;
            }
            else
            {
                Debug.Log("공격 범위 밖에 적이 없습니다. ChasingState로 전환합니다.");
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
        }
    }
}
