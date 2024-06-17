using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerSO Data; // PlayerSO�� ���� ������ �ʵ�

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

        // Ȯ�ο� �����
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
            Data.OnLevelChanged += HandleLevelChanged; // �̺�Ʈ �ڵ鷯 ���
            Data.OnExperienceChanged += HandleExperienceChanged; // ����ġ ���� �̺�Ʈ �ڵ鷯 ���
            Data.OnGoldChanged += HandleGoldChanged; // ��� ���� �̺�Ʈ �ڵ鷯 ���
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
        Debug.Log($"�÷��̾� ������ ����Ǿ����ϴ�. ���ο� ����: {newLevel}");
    }

    private void HandleExperienceChanged(int newExperience)
    {
        Debug.Log($"�÷��̾� ����ġ�� ����Ǿ����ϴ�. ���� ����ġ: {newExperience}");
    }

    private void HandleGoldChanged(int newGold)
    {
        Debug.Log($"�÷��̾� ��尡 ����Ǿ����ϴ�. ���� ���: {newGold}");
    }

    // ����ġ �߰� �޼���
    public void AddExperience(int amount)
    {
        if (Data != null)
        {
            Data.Experience += amount;
            Debug.Log($"����ġ �߰�: {amount}, ���� ����ġ: {Data.Experience}");

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

    // ������ �޼���
    private void LevelUp()
    {
        if (Data != null)
        {
            Data.Level++;
            Data.Experience -= Data.ExperienceToNextLevel;
            Data.ExperienceToNextLevel = Mathf.RoundToInt(Data.ExperienceToNextLevel * 1.1f);

            Debug.Log($"���� ��! ���� ����: {Data.Level}, ���� �������� �ʿ��� ����ġ: {Data.ExperienceToNextLevel}");
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
            Debug.Log($"�÷��̾ {amount} ��带 ȹ���߽��ϴ�. ���� ���: {Data.Gold}");
        }
        else
        {
            Debug.LogError("PlayerSO reference is null in Player component. Cannot add gold.");
        }
    }
}
