using System.Collections;
using UnityEngine;

public class ParentDemoAttackState : EnemyState
{
    Enemy_ParentDemon enemy;

    public ParentDemoAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_ParentDemon _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StartCoroutine(WaitFireCarpet(enemy.waitFireCarpet));
    }

    public IEnumerator WaitFireCarpet(float second)
    {
        yield return new WaitForSeconds(second);

        enemy.enemyCaFireCarpet();

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
