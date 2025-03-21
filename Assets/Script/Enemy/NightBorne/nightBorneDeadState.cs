using System.Collections;
using UnityEngine;


public class nightBorneDeadState : EnemyState
{
    protected Enemy_NightBorne enemy;
    public nightBorneDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Object.Destroy(enemy.gameObject);
    }
    public override void Enter()
    {
        base.Enter();

        //enemy.animator.SetBool(enemy.lastAnimBoolName, true);
        //enemy.animator.speed = 0;
        //enemy.cd.enabled = false;

        //stateTimer = .15f;
        AudioManager.instance.PlaySFX(54, enemy.transform);
        enemy.chacracterStats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 10);
        }
    }
    public override void Exit()
    {
        base.Exit();
        enemy.chacracterStats.MakeInvincible(true);
    }
}
