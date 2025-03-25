using System.Collections;
using UnityEngine;


public class Archer2BattleState : EnemyState
{
    Enemy_Archer2 enemy;
    Transform player;
    private int moveDir;
    private int randomDashOrJump;
    public Archer2BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer2 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        randomDashOrJump = Random.Range(0, 2);
    }
    public override void Update()
    {
        base.Update();



        if (enemy.isPlayerDetected())
        {

            stateTimer = enemy.battleTime;

            if (enemy.isPlayerDetected().distance < enemy.safeDistance)
            {
                if (canJump() && randomDashOrJump == 0)
                {
                    stateMachine.ChangeState(enemy.jumpState);
                }
                if (canDash() && randomDashOrJump == 1)
                {
                    stateMachine.ChangeState(enemy.dashState);
                }
            }

            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {

            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
        {
            enemy.Flip();
        }
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
        {
            enemy.Flip();
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

        //enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
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

    private bool canJump()
    {
        if (enemy.GroundBehindCheck() == false || enemy.WallBehinCheck() == true)
        {
            return false;
        }

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;

        }
        return false;
    }
    private bool canDash()
    {
        if (enemy.GroundBehindCheck() == false || enemy.WallBehinCheck() == true)
        {
            return false;
        }

        if (Time.time >= enemy.lastTimeDashed + enemy.DashCooldown)
        {
            enemy.lastTimeDashed = Time.time;
            return true;

        }
        return false;
    }
}
