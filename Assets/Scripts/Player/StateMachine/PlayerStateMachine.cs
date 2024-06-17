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
        if (player == null)
        {
            Debug.LogError("PlayerStateMachine: Player 객체가 null입니다.");
            return;
        }

        this.Player = player;

        if (player.Data == null)
        {
            Debug.LogError("PlayerStateMachine: player.Data가 null입니다.");
            return;
        }

        // 예시: Target 설정
        Target = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<Health>();
        // 예시: EnemyTracker 초기화
        EnemyTracker = GameObject.FindObjectOfType<EnemyTracker>();

        // 예시: 카메라 설정
        MainCamTransform = Camera.main?.transform;

        // 상태 초기화
        GroundState = new PlayerGroundState(this);
        ChasingState = new PlayerChasingState(this);
        AttackState = new PlayerAttackState(this);

        // 예시: 움직임 속도 및 회전 감소 설정
        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotaionDamping;
    }
}
