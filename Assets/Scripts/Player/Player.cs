using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public AnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    private PlayerStateMachine stateMachine;
    public Health health { get; private set; }
    [field: SerializeField] public Weapon Weapon { get; private set; }

    // 새로운 필드 추가
    public int gold = 0;

    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnGoldChanged;
    [field: SerializeField] public int Level { get; private set; } = 1; // 초기 레벨 1
    [field: SerializeField] public int Experience { get; private set; } = 0; // 초기 경험치 0
    [field: SerializeField] public int ExperienceToNextLevel { get; private set; } = 100; // 다음 레벨업까지 필요한 경험치

    public NavMeshAgent NavMeshAgent { get; private set; }

    

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        Controller = GetComponent<CharacterController>();
        stateMachine = new PlayerStateMachine(this);

    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.ChasingState);
        health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    // 경험치 추가 메서드
    public void AddExperience(int amount)
    {
        Experience += amount;
        Debug.Log($"경험치 추가: {amount}, 현재 경험치: {Experience}");

        // 경험치 변경 이벤트 호출
        OnExperienceChanged?.Invoke(Experience);

        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        ExperienceToNextLevel = Mathf.RoundToInt(ExperienceToNextLevel * 1.1f);

        // 레벨 변경 이벤트 호출
        OnLevelChanged?.Invoke(Level);

        // 남은 경험치 업데이트
        OnExperienceChanged?.Invoke(Experience);

        Debug.Log($"레벨 업! 현재 레벨: {Level}, 다음 레벨까지 필요한 경험치: {ExperienceToNextLevel}");
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"플레이어가 {amount} 골드를 획득했습니다. 현재 골드: {gold}");

        // 골드 변경 이벤트 호출
        OnGoldChanged?.Invoke(gold);
    }
}
