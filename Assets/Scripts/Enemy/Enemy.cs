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

    // NavMeshAgent 및 랜덤 이동 관련 필드 추가
    private NavMeshAgent agent;
    [SerializeField] private float patrolRadius = 10f; // 이동 반경
    [SerializeField] private float patrolTime = 5f; // 이동 시간 간격
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

        // NavMeshAgent 초기화
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        health.OnDie += OnDie;

        // 첫 번째 랜덤 위치로 이동 시작
        SetNewDestination();
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();

        // 일정 시간 간격으로 새로운 랜덤 목적지 설정
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
        // 새로운 랜덤 위치 설정
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

}
