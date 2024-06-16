using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    private EnemyStateMachine stateMachine;

    public Health health { get; private set; }
    [field: SerializeField] public Weapon Weapon { get; private set; }

    // NavMeshAgent �� ���� �̵� ���� �ʵ� �߰�
    private NavMeshAgent agent;
    [SerializeField] private float patrolRadius = 10f; // �̵� �ݰ�
    [SerializeField] private float patrolTime = 5f; // �̵� �ð� ����
    private float patrolTimer;

    [SerializeField] private int goldDropMin = 10;
    [SerializeField] private int goldDropMax = 20;
    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        Controller = GetComponent<CharacterController>();
        stateMachine = new EnemyStateMachine(this);

        // NavMeshAgent �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        health.OnDie += OnDie;

        // ù ��° ���� ��ġ�� �̵� ����
        SetNewDestination();
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();

        // ���� �ð� �������� ���ο� ���� ������ ����
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolTime)
        {
            SetNewDestination();
            patrolTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        gameObject.SetActive(false);

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.AddExperience(Data.ExperiencePoints);
            int goldAmount = Random.Range(goldDropMin, goldDropMax + 1);
            player.AddGold(goldAmount);
        }
    }

    private void SetNewDestination()
    {
        // ���ο� ���� ��ġ ����
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

}
