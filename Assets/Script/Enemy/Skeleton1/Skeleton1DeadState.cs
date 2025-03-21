using System.Collections;
using UnityEngine;


public class Skeleton1DeadState : EnemyState
{
    private Enemy_Skeleton1 enemy;
    public Skeleton1DeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
