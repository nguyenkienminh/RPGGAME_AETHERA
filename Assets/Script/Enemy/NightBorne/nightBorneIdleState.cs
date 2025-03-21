using System.Collections;
using UnityEngine;

namespace Assets.Script.Enemy.NightBorne
{
    public class nightBorneIdleState : nightBorneGroundState
    {
        public nightBorneIdleState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateTimer = enemy.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
            AudioManager.instance.PlaySFX(20, enemy.transform);

        }

        public override void Update()
        {
            base.Update();

            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            
        }
    }
}