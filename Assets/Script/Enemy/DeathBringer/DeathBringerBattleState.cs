using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    Transform player;
    private int moveDir;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        //if (player.GetComponent<Player>().isDead)
        //{
        //    stateMachine.ChangeState(enemy.moveState);
        //}
        AudioManager.instance.PlaySFX(55, enemy.transform);
    }
    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                bool canAttackResult = canAttack();
                if (canAttackResult)
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.idleState);
                }
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
