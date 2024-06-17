using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerSO Data; // PlayerSO를 직접 참조할 필드

    [Header("Animations")]
    [SerializeField] public AnimationData AnimationData;
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    private PlayerStateMachine stateMachine;
    public Health Health { get; private set; }
    [SerializeField] public Weapon Weapon;

    public NavMeshAgent NavMeshAgent { get; private set; }

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Health = GetComponent<Health>();
        Controller = GetComponent<CharacterController>();
        stateMachine = new PlayerStateMachine(this);

        // 확인용 디버깅
        if (Weapon == null)
        {
            Debug.LogError("Player: Weapon is not assigned.");
        }
        if (Data == null)
        {
            Debug.LogError("Player: Data is not assigned.");
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.ChasingState);
        Health.OnDie += OnDie;

        if (Data != null)
        {
            Data.OnLevelChanged += HandleLevelChanged; // 이벤트 핸들러 등록
            Data.OnExperienceChanged += HandleExperienceChanged; // 경험치 변경 이벤트 핸들러 등록
            Data.OnGoldChanged += HandleGoldChanged; // 골드 변경 이벤트 핸들러 등록
        }
        else
        {
            Debug.LogError("PlayerSO reference is null in Player component.");
        }
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

    private void HandleLevelChanged(int newLevel)
    {
        Debug.Log($"플레이어 레벨이 변경되었습니다. 새로운 레벨: {newLevel}");
    }

    private void HandleExperienceChanged(int newExperience)
    {
        Debug.Log($"플레이어 경험치가 변경되었습니다. 현재 경험치: {newExperience}");
    }

    private void HandleGoldChanged(int newGold)
    {
        Debug.Log($"플레이어 골드가 변경되었습니다. 현재 골드: {newGold}");
    }

    // 경험치 추가 메서드
    public void AddExperience(int amount)
    {
        if (Data != null)
        {
            Data.Experience += amount;
            Debug.Log($"경험치 추가: {amount}, 현재 경험치: {Data.Experience}");

            if (Data.Experience >= Data.ExperienceToNextLevel)
            {
                LevelUp();
            }
        }
        else
        {
            Debug.LogError("PlayerSO reference is null in Player component. Cannot add experience.");
        }
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        if (Data != null)
        {
            Data.Level++;
            Data.Experience -= Data.ExperienceToNextLevel;
            Data.ExperienceToNextLevel = Mathf.RoundToInt(Data.ExperienceToNextLevel * 1.1f);

            Debug.Log($"레벨 업! 현재 레벨: {Data.Level}, 다음 레벨까지 필요한 경험치: {Data.ExperienceToNextLevel}");
        }
        else
        {
            Debug.LogError("PlayerSO reference is null in Player component. Cannot level up.");
        }
    }

    public void AddGold(int amount)
    {
        if (Data != null)
        {
            Data.Gold += amount;
            Debug.Log($"플레이어가 {amount} 골드를 획득했습니다. 현재 골드: {Data.Gold}");
        }
        else
        {
            Debug.LogError("PlayerSO reference is null in Player component. Cannot add gold.");
        }
    }
}
