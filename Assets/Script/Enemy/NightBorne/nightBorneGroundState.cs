using System.Collections;
using UnityEngine;


public class nightBorneGroundState : EnemyState
{
    protected Enemy_NightBorne enemy;

    protected Transform player;
    public nightBorneGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_NightBorne _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.arrowDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}
