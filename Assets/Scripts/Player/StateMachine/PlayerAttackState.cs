using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    bool alreadyAppliedForce;
    bool alreadyAppliedDealing;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.BaseAttackParameterName);

        alreadyAppliedForce = false;
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

        // stateMachine.Player가 null인 경우 처리
        if (stateMachine.Player == null)
        {
            Debug.LogError("PlayerAttackState: stateMachine.Player is null.");
            return;
        }

        // stateMachine.Player.Data가 null인 경우 처리
        if (stateMachine.Player.Data == null)
        {
            Debug.LogError("PlayerAttackState: stateMachine.Player.Data is null.");
            return;
        }

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Player.Data.Dealing_Start_TransitionTime)
            {
                int randomDamage = Random.Range(-5, 5);
                stateMachine.Player.Weapon.SetAttack(stateMachine.Player.Data.Damage + randomDamage);
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
            // 공격이 끝나면 다시 추적 상태로 전환
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

}
