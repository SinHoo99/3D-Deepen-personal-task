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
            Debug.LogError("PlayerStateMachine: Player ��ü�� null�Դϴ�.");
            return;
        }

        this.Player = player;

        if (player.Data == null)
        {
            Debug.LogError("PlayerStateMachine: player.Data�� null�Դϴ�.");
            return;
        }

        // ����: Target ����
        Target = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<Health>();
        // ����: EnemyTracker �ʱ�ȭ
        EnemyTracker = GameObject.FindObjectOfType<EnemyTracker>();

        // ����: ī�޶� ����
        MainCamTransform = Camera.main?.transform;

        // ���� �ʱ�ȭ
        GroundState = new PlayerGroundState(this);
        ChasingState = new PlayerChasingState(this);
        AttackState = new PlayerAttackState(this);

        // ����: ������ �ӵ� �� ȸ�� ���� ����
        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotaionDamping;
    }
}
