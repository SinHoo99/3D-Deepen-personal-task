using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public Transform MainCamTransform { get; set; }
    public Health Target { get; set; }
    public PlayerIdleState IdleState { get; }
    public bool IsAttacking { get; set; }
    public PlayerChasingState ChasingState { get; }
    public PlayerAttackState AttackState { get; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Health>();

        this.Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        ChasingState = new PlayerChasingState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotaionDamping;
    }
}
