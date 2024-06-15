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
    public bool IsAttacking { get; set; }

    public EnemyTracker EnemyTracker { get; private set; }
    public PlayerChasingState ChasingState { get; }
    public PlayerAttackState AttackState { get; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Health>();
        EnemyTracker = GameObject.FindObjectOfType<EnemyTracker>();

        this.Player = player;
        MainCamTransform = Camera.main.transform;

        ChasingState = new PlayerChasingState(this);
        AttackState = new PlayerAttackState(this); // 추가: AttackState 설정

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotaionDamping;
    }
    public string GetCurrentStateName()
    {
        return currentState != null ? currentState.GetType().Name : "None";
    }
}
