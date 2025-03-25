using System.Collections;
using UnityEngine;

public class FlyEyeAttackState : EnemyState
{
    private Enemy_FlyEye enemy;

    public FlyEyeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_FlyEye _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        enemy.RushAndAttackPlayer();

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

        if (triggerCalled && enemy.stateMachine.currentState == this)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}

