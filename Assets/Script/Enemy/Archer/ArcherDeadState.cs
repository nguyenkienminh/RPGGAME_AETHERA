using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Archer
{
    public class ArcherDeadState : EnemyState
    {
        private Enemy_Archer enemy;
        public ArcherDeadState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            this.enemy = _enemy;
        }
        public override void Enter()
        {
            base.Enter();

            //enemy.animator.SetBool(enemy.lastAnimBoolName, true);
            //enemy.animator.speed = 0;
            //enemy.cd.enabled = false;

            //stateTimer = .15f;
            AudioManager.instance.PlaySFX(53, enemy.transform);
            enemy.chacracterStats.MakeInvincible(true);
        }

        public override void Update()
        {
            base.Update();

            if (stateTimer > 0)
            {
                rb.linearVelocity = new Vector2(0, 10);
            }
        }
        public override void Exit()
        {
            base.Exit();
            enemy.chacracterStats.MakeInvincible(false);
        }
    }

}