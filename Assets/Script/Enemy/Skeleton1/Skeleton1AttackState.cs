using System.Collections;
using UnityEngine;


public class Skeleton1AttackState : EnemyState
{
    Enemy_Skeleton1 enemy;
    private int attackType;

    public Skeleton1AttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        attackType = Random.Range(0,2);
        enemy.animator.SetFloat("AttackType", attackType); 
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
