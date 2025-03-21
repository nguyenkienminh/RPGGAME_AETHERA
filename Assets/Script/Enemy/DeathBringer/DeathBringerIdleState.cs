using System.Collections;
using UnityEngine;


public class DeathBringerIdleState : EnemyState
{
    Enemy_DeathBringer enemy;
    private Transform player;
    public DeathBringerIdleState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(21, enemy.transform);

    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(enemy.transform.position, enemy.transform.position) < 7)
        {
            enemy.bossFightBegun = true;
        }

        if (stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
      
    }

}
