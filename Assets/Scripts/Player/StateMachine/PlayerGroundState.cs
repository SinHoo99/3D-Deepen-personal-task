using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{

    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.IsAttacking)
        {
            return;
        }
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

    }
}
