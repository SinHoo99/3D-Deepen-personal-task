using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Enemy.Data.GroundData;
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
        stateMachine.Enemy.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Enemy.Animator.SetBool(animatorHash, false);
    }
    private void Move()
    {
        Vector3 movementDircetion = GetMovementDirection();

        Move(movementDircetion);

        Rotate(movementDircetion);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position);

        return dir;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        stateMachine.Enemy.Controller.Move((direction * movementSpeed) * Time.deltaTime);
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

            Transform playerTransform = stateMachine.Enemy.transform;

            // targetRotation을 수정된 direction을 기반으로 생성
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 플레이어를 targetRotation으로 회전
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected float GetNomalizedTime(Animator animator, string tag)
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
    protected bool IsinChsingRange()
    {
        if (stateMachine.Target.IsDie) return false;

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Enemy.Data.EnemyChasingRange * stateMachine.Enemy.Data.EnemyChasingRange;
    }

    
}
