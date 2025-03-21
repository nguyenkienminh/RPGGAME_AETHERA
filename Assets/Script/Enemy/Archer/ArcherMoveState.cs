using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Archer
{
    public class ArcherMoveState : ArcherGroundedState
    {
        public ArcherMoveState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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

            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

            //dang le k co
            if (enemy.isWallDetected() || !enemy.isGroundDetected())
            {
                enemy.Flip();
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}