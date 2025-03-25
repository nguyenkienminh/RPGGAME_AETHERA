using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Archer
{
    public class ArcherAttackState : EnemyState
    {
        private Enemy_Archer enemy;
        public ArcherAttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void Exit()
        {
            base.Exit();

            enemy.lastTimeAttacked = Time.time;
        }

        public override void Update()
        {
            base.Update();

            enemy.SetZeroVelocity();

            if (triggerCalled)
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}