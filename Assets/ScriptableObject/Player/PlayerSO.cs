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

    [Header("Combat Data")]
    [SerializeField] private int damage;

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

    private Action<int> onExperienceChanged;
    public event Action<int> OnExperienceChanged
    {
        add => onExperienceChanged += value;
        remove => onExperienceChanged -= value;
    }

    private Action<int> onLevelChanged;
    public event Action<int> OnLevelChanged
    {
        add => onLevelChanged += value;
        remove => onLevelChanged -= value;
    }

    private Action<int> onGoldChanged;
    public event Action<int> OnGoldChanged
    {
        add => onGoldChanged += value;
        remove => onGoldChanged -= value;
    }

    [SerializeField] private int gold = 0;
    public int Gold
    {
        get => gold;
        set
        {
            if (gold != value)
            {
                gold = value;
                onGoldChanged?.Invoke(gold); // Gold 변경 시 이벤트 호출
            }
        }
    }

    [SerializeField] private int level = 1; // 초기 레벨 1
    public int Level
    {
        get => level;
        set
        {
            if (level != value)
            {
                level = value;
                onLevelChanged?.Invoke(level); // Level 변경 시 이벤트 호출
            }
        }
    }

    [SerializeField] private int experience = 0; // 초기 경험치 0
    public int Experience
    {
        get => experience;
        set
        {
            if (experience != value)
            {
                experience = value;
                onExperienceChanged?.Invoke(experience); // Experience 변경 시 이벤트 호출
            }
        }
    }

    [SerializeField] private int experienceToNextLevel = 100; // 다음 레벨업까지 필요한 경험치
    public int ExperienceToNextLevel
    {
        get => experienceToNextLevel;
        set => experienceToNextLevel = value;
    }

    [SerializeField] private float playerChasingRange;
    public float PlayerChasingRange
    {
        get => playerChasingRange;
        set => playerChasingRange = value;
    }

    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    [SerializeField, Range(0f, 1f)] private float dealing_Start_TransitionTime;
    public float Dealing_Start_TransitionTime
    {
        get => dealing_Start_TransitionTime;
        set => dealing_Start_TransitionTime = value;
    }

    [SerializeField, Range(0f, 1f)] private float dealing_End_TransitionTime;
    public float Dealing_End_TransitionTime
    {
        get => dealing_End_TransitionTime;
        set => dealing_End_TransitionTime = value;
    }

    [SerializeField] private PlayerGroundData groundData;
    public PlayerGroundData GroundData
    {
        get => groundData;
        set => groundData = value;
    }

    [SerializeField] private PlayerAttackData attackData;
    public PlayerAttackData AttackData
    {
        get => attackData;
        set => attackData = value;
    }
    // To JSON for saving data
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // From JSON for loading data
    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
