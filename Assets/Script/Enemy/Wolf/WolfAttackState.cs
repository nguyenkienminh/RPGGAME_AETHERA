using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Wolf
{
    public class WolfAttackState : EnemyState
    {
        private Enemy_Woft enemy;
        public WolfAttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Woft _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            enemy = _enemy;
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