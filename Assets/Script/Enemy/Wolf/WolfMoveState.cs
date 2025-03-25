using System.Collections;
using UnityEngine;


public class WolfMoveState : WolfGroundState
{
    public WolfMoveState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Woft _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
