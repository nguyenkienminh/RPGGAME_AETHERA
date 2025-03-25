using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Archer
{
    public class ArcherJumpState : EnemyState
    {
        private Enemy_Archer enemy;
        public ArcherJumpState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            enemy = _enemy;
        }
        public override void Enter()
        {
            base.Enter();

            rb.linearVelocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            enemy.animator.SetFloat("yVelocity", rb.linearVelocity.y);

            if (rb.linearVelocity.y < 0 && enemy.isGroundDetected())
            {
                stateMachine.ChangeState(enemy.battleState);
            }

        }

    }
}