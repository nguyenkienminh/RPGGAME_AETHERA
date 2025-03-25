using UnityEngine;

public class slimStunnedState : EnemyState
{
    protected Enemy_Slime enemy;
    public slimStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;

        rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
        enemy.chacracterStats.MakeInvincible(true);
        Debug.Log("Enter" + enemy.chacracterStats.isInvincible);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.chacracterStats.MakeInvincible(false);
        Debug.Log("Exit" + enemy.chacracterStats.isInvincible);

    }

    public override void Update()
    {
        base.Update();


        if (rb.linearVelocity.y < .1f && enemy.isGroundDetected())
        {
            enemy.fx.Invoke("CancelColorChange", 0);
            enemy.animator.SetTrigger("StunFold");
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
