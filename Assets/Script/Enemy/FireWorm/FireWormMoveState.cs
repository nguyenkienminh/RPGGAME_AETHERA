using System.Collections;
using UnityEngine;

public class FireWormMoveState : FireWormGroundState
{
    public FireWormMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_FireWorm enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.linearVelocity.y);

        //dang le k co
        if (enemy.isWallDetected())
        {
            stateMachine.ChangeState(enemy.idleState); // Tạo khoảng nghỉ trước khi di chuyển lại
        }
        else if (!enemy.isGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
        }

    }
}
