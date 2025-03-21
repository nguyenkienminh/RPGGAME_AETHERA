 using System.Collections;
using UnityEngine;


public class WolfDeadState : EnemyState
{
    Enemy_Woft enemy;
    public WolfDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Woft _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        AudioManager.instance.PlaySFX(53, enemy.transform);
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
