using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
