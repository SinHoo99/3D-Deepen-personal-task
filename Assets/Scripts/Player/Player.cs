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

    // ���ο� �ʵ� �߰�
    public int gold = 0;

    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnGoldChanged;
    [field: SerializeField] public int Level { get; private set; } = 1; // �ʱ� ���� 1
    [field: SerializeField] public int Experience { get; private set; } = 0; // �ʱ� ����ġ 0
    [field: SerializeField] public int ExperienceToNextLevel { get; private set; } = 100; // ���� ���������� �ʿ��� ����ġ

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

    // ����ġ �߰� �޼���
    public void AddExperience(int amount)
    {
        Experience += amount;
        Debug.Log($"����ġ �߰�: {amount}, ���� ����ġ: {Experience}");

        // ����ġ ���� �̺�Ʈ ȣ��
        OnExperienceChanged?.Invoke(Experience);

        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    // ������ �޼���
    private void LevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        ExperienceToNextLevel = Mathf.RoundToInt(ExperienceToNextLevel * 1.1f);

        // ���� ���� �̺�Ʈ ȣ��
        OnLevelChanged?.Invoke(Level);

        // ���� ����ġ ������Ʈ
        OnExperienceChanged?.Invoke(Experience);

        Debug.Log($"���� ��! ���� ����: {Level}, ���� �������� �ʿ��� ����ġ: {ExperienceToNextLevel}");
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"�÷��̾ {amount} ��带 ȹ���߽��ϴ�. ���� ���: {gold}");

        // ��� ���� �̺�Ʈ ȣ��
        OnGoldChanged?.Invoke(gold);
    }
}
