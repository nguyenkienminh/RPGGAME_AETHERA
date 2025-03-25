using System.Collections;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Enemy_Archer enemy;
    Transform player;
    private int moveDir;
    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<Player>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
    public override void Update()
    {
        base.Update();



        if (enemy.isPlayerDetected())
        {

            stateTimer = enemy.battleTime;

            if(enemy.isPlayerDetected().distance < enemy.safeDistance)
            {
                if (canJump())
                {
                    stateMachine.ChangeState(enemy.jumpState);
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
        if(enemy.GroundBehindCheck() == false || enemy.WallBehinCheck() == true)
        {
            return false;
        }

        if(Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;

        }
        return false;
    }
}
