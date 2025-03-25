using UnityEngine;

public class Skeleton2AttackState : EnemyState
{
    Enemy_Skeleton2 enemy;
    private int attackType;
    public Skeleton2AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        attackType = Random.Range(0, 2);
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
