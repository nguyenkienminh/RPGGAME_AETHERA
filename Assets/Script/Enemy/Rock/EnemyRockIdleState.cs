using System.Collections;
using UnityEngine;


public class EnemyRockIdleState : EnemyState
{
    Enemy_Rock enemy;
    public EnemyRockIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Rock _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();       
    }

    public override void Exit()
    {
        base.Exit();
    }

}
