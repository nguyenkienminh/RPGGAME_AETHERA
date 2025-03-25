using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.Shady
{
    public class ShadyDeadState : EnemyState
    {
        private Enemy_Shady enemy;
        public ShadyDeadState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            enemy = _enemy;
        }
        public override void Enter()
        {
            base.Enter();

        }

        public override void Update()
        {
            base.Update();

            if (stateTimer > 0)
            {
                rb.linearVelocity = new Vector2(0, 10);
            }

            if (triggerCalled)
            {
                enemy.SelfDestroy();
            }
        }
        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            Object.Destroy(enemy.gameObject);
        }

     
    }
}