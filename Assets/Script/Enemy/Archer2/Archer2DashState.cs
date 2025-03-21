using System.Collections;
using UnityEngine;


public class Archer2DashState : EnemyState
{
    private Enemy_Archer2 enemy;
    public Archer2DashState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(enemy.dashVelocity.x * -enemy.facingDir, enemy.dashVelocity.y);

    }

    public override void Exit()
    {
        base.Exit();


    }

    public override void Update()
    {
        base.Update();

        if (enemy.isGroundDetected())
        {
            enemy.StartCoroutine(DelayExitDash(enemy.delayDash));
        }
    }
    public IEnumerator DelayExitDash(float second)
    {
        yield return new WaitForSeconds(second); stateMachine.ChangeState(enemy.battleState);
    }
}
