using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerGroundData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 25f)] public float BaseRotaionDamping { get; private set; } = 1f;

    [field: Header("IdleData")]

    [field: Header("WalkData")]
    [field: SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;
    [field: Header("RunData")]
    [field: SerializeField][field: Range(0f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;
}
[Serializable]

public class PlayerAttackData
{
    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; }
    public int GetAttackInfoCount() { return AttackInfoDatas.Count; }
    public AttackInfoData GetAttackInfo(int index) { return AttackInfoDatas[index]; }
}
[Serializable]
public class AttackInfoData
{
    [field: SerializeField] public string AttackName { get; private set; }

    [field: SerializeField] public int Damage;
    
}

[CreateAssetMenu(fileName = "Player", menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    public event Action OnDamageChanged;
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAttackData AttackData { get; private set; }

    [Header("Combat Data")]
    [field: SerializeField] public int damage;
    [SerializeField]
    public int Damage
    {
        get => damage;
        set
        {
            if (damage != value)
            {
                damage = value;
                OnDamageChanged?.Invoke(); // Damage 변경 시 이벤트 호출
            }
        }
    }

    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }
}
