using System.Collections;
using UnityEngine;


public class Wizard2AttackState : EnemyState
{
    Enemy_Wizard2 enemy;
    public Wizard2AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Wizard2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

