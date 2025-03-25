using System.Collections;
using UnityEngine;


public class nightBorneBattleState : EnemyState
{
    Enemy_NightBorne enemy;
    Transform player;
    private int moveDir;

    private bool flippedOnce;
    public nightBorneBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<Player>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        flippedOnce = false;

    }
    public override void Update()
    {
        base.Update();


        enemy.animator.SetFloat("xVelocity", enemy.rb.linearVelocity.x);

        if (enemy.isPlayerDetected())
        {

            stateTimer = enemy.battleTime;

            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
            else if (enemy.isPlayerDetected().distance < enemy.attackCheckRadius)
            {
                if (enemy.CanThrowball())
                {
                    stateMachine.ChangeState(enemy.attackSpecialState);
                }
            }
        }
        else
        {
            if (flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }

            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackDistance - 1f)
        {
            return;
        }

        if (!enemy.isGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }
        if (enemy.isWallDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
