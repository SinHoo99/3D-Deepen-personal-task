using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        if (stateMachine.Player == null)
        {
            Debug.LogError("PlayerStateMachine의 Player가 null입니다.");
        }
        else if (stateMachine.Player.Data == null)
        {
            Debug.LogError("PlayerStateMachine의 Player.Data가 null입니다.");
        }
        else if (stateMachine.Player.Data.GroundData == null)
        {
            Debug.LogError("PlayerStateMachine의 Player.Data.GroundData가 null입니다.");
        }

        // 위에서 null 체크를 통과하면 groundData에 할당
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public void HandleInput()
    {

    }

    public virtual void PhysicUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 dir = stateMachine.Target.transform.position - stateMachine.Player.transform.position;

        // y축 이동을 막기 위해 y 값을 0으로 설정
        dir.y = 0;

        // direction 벡터를 정규화
        dir.Normalize();

        return dir;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        // 현재 위치에서 y 값을 고정
        Vector3 currentPosition = stateMachine.Player.transform.position;

        // 이동할 위치 계산
        Vector3 move = (direction * movementSpeed) * Time.deltaTime;

        // 최종적으로 y 좌표를 현재 위치의 y 값으로 설정
        Vector3 finalPosition = currentPosition + move;
        finalPosition.y = currentPosition.y; // y 좌표 고정

        // 이동할 양만큼 Controller를 이동시킴
        stateMachine.Player.Controller.Move(move);
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // y축 회전을 막기 위해 y 값을 0으로 설정
            direction.y = 0;

            // direction 벡터를 정규화
            direction.Normalize();

            Transform playerTransform = stateMachine.Player.transform;

            // targetRotation을 수정된 direction을 기반으로 생성
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 플레이어를 targetRotation으로 회전
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected bool IsInChasingRange()
    {
        if (stateMachine.Target == null || stateMachine.Target.IsDie)
        {
            UpdateTarget(); // 다른 적을 재추적
            return false;   // 현재는 추적 범위에 없음
        }

        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return enemyDistanceSqr <= stateMachine.Player.Data.PlayerChasingRange * stateMachine.Player.Data.PlayerChasingRange;
    }

    protected virtual void UpdateTarget()
    {
        // 기본 클래스에서는 추적과 관련된 로직이 없으므로 구현하지 않음.
    }
}
