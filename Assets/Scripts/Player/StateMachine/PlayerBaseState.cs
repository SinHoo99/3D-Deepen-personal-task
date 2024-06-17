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
            Debug.LogError("PlayerStateMachine�� Player�� null�Դϴ�.");
        }
        else if (stateMachine.Player.Data == null)
        {
            Debug.LogError("PlayerStateMachine�� Player.Data�� null�Դϴ�.");
        }
        else if (stateMachine.Player.Data.GroundData == null)
        {
            Debug.LogError("PlayerStateMachine�� Player.Data.GroundData�� null�Դϴ�.");
        }

        // ������ null üũ�� ����ϸ� groundData�� �Ҵ�
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

        // y�� �̵��� ���� ���� y ���� 0���� ����
        dir.y = 0;

        // direction ���͸� ����ȭ
        dir.Normalize();

        return dir;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        // ���� ��ġ���� y ���� ����
        Vector3 currentPosition = stateMachine.Player.transform.position;

        // �̵��� ��ġ ���
        Vector3 move = (direction * movementSpeed) * Time.deltaTime;

        // ���������� y ��ǥ�� ���� ��ġ�� y ������ ����
        Vector3 finalPosition = currentPosition + move;
        finalPosition.y = currentPosition.y; // y ��ǥ ����

        // �̵��� �縸ŭ Controller�� �̵���Ŵ
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
            // y�� ȸ���� ���� ���� y ���� 0���� ����
            direction.y = 0;

            // direction ���͸� ����ȭ
            direction.Normalize();

            Transform playerTransform = stateMachine.Player.transform;

            // targetRotation�� ������ direction�� ������� ����
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // �÷��̾ targetRotation���� ȸ��
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
            UpdateTarget(); // �ٸ� ���� ������
            return false;   // ����� ���� ������ ����
        }

        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return enemyDistanceSqr <= stateMachine.Player.Data.PlayerChasingRange * stateMachine.Player.Data.PlayerChasingRange;
    }

    protected virtual void UpdateTarget()
    {
        // �⺻ Ŭ���������� ������ ���õ� ������ �����Ƿ� �������� ����.
    }
}
