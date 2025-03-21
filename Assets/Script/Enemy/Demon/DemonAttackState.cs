using System.Collections;
using UnityEngine;


public class DemonAttackState : EnemyState
{
    private Enemy_Demon enemy;
    public DemonAttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Demon _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.StartCoroutine(DelayedAttack());

    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }
    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(enemy.waitSecondToSpawn);
        enemy.CanExploreDemon();
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

