using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public PlayerGroundState GroundState { get; }
    public PlayerChasingState ChasingState { get; }
    public PlayerAttackState AttackState { get; }

    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public Transform MainCamTransform { get; set; }
    public Health Target { get; set; }
    public bool IsAttacking { get; set; }

    public EnemyTracker EnemyTracker { get; private set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        Target = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<Health>();
        EnemyTracker = GameObject.FindObjectOfType<EnemyTracker>();

        MainCamTransform = Camera.main.transform;

        GroundState = new PlayerGroundState(this);
        ChasingState = new PlayerChasingState(this);
        AttackState = new PlayerAttackState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotaionDamping;
    }

    public string GetCurrentStateName()
    {
        return currentState != null ? currentState.GetType().Name : "None";
    }
}
