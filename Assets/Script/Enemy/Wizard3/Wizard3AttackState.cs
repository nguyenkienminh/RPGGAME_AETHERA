using System.Collections;
using UnityEngine;


public class Wizard3AttackState : EnemyState
{
    Enemy_Wizard3 enemy;
    public Wizard3AttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard3 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
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
        enemy.CanExploreWizard3(); 
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
