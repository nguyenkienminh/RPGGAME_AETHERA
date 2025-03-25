using System.Collections;
using UnityEngine;


public class FireWormAttackState : EnemyState
{
    Enemy_FireWorm enemy;   
    public FireWormAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_FireWorm _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        enemy.StartCoroutine(DeplayCanMoveAndExplosive(enemy.deplay));

    }

    public IEnumerator DeplayCanMoveAndExplosive(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.enemyCanMoveAndExplosive();

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
