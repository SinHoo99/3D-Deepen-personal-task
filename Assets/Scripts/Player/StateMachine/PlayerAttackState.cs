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


        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime < 1f)
        {

            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_Start_TransitionTime)
            {
                stateMachine.Player.Weapon.SetAttack(stateMachine.Player.Data.Damage);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }
        }
        else
        {
            if (IsInChasingRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }

        }
    }
}
