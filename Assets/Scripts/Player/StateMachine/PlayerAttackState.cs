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
            Debug.LogError("Animator�� �������� �ʾҽ��ϴ�.");
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
            Debug.LogError("Animator�� �������� �ʾҽ��ϴ�.");
            return;
        }

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        Debug.Log($"Normalized Time: {normalizedTime}");

        if (normalizedTime < 1f)
        {
            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_Start_TransitionTime)
            {
                Debug.Log("���� ����");
                stateMachine.Player.Weapon.SetAttack(stateMachine.Player.Data.Damage);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }

            // ���� ������� ChasingState�� ��ȯ
            if (stateMachine.Target == null || stateMachine.Target.IsDie || !stateMachine.ChasingState.IsInAttackRange())
            {
                Debug.Log("���� ��������ϴ�. ChasingState�� ��ȯ�մϴ�.");
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
        }
        else // normalizedTime�� 1 �̻��̸� ������ ���� ����
        {
            if (stateMachine.ChasingState.IsInAttackRange())
            {
                Debug.Log("���� ���� �ȿ� ���� �ֽ��ϴ�. AttackState�� �����մϴ�.");
                return;
            }
            else
            {
                Debug.Log("���� ���� �ۿ� ���� �����ϴ�. ChasingState�� ��ȯ�մϴ�.");
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
        }
    }
}
