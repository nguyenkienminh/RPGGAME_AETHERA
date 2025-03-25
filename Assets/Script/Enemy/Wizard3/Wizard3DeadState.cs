using System.Collections;
using UnityEngine;


public class Wizard3DeadState : EnemyState
{
    private Enemy_Wizard3 enemy;
    public Wizard3DeadState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard3 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Object.Destroy(enemy.gameObject);
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(58, enemy.transform);
        enemy.chacracterStats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.chacracterStats.MakeInvincible(false);

    }

    public override void Update()
    {
        base.Update();
    }
}
