using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Enemy_Shady enemy;
    Transform player;
    private int moveDir;
    private float defaultSpeed;
    private bool isExploding;
    private bool shouldExplode;

    public ShadyBattleState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleStateMoveSpeed;
        player = PlayerManager.instance.player.transform;
        isExploding = false;
        shouldExplode = false;

        if (player.GetComponent<Player>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        enemy.chacracterStats.MakeInvincible(true);
    }
    
    public override void Update()
    {
        base.Update();
        if (isExploding) return;

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < 5f)
        {
            stateTimer = enemy.battleTime;
            shouldExplode = true;

        }
        else if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 10)
        {
            stateMachine.ChangeState(enemy.idleState);
            shouldExplode = false; 
        }

        if (shouldExplode && !isExploding)
        {
            enemy.StartCoroutine(ExplodeAfterDelay());
        }

        moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;

        if (!enemy.isGroundDetected() || enemy.isWallDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            shouldExplode = false; 
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    private IEnumerator ExplodeAfterDelay()
    {
        enemy.boomText.gameObject.SetActive(true);
        enemy.StartCoroutine(BlinkRedEffect()); 

        yield return new WaitForSeconds(2f); 

        isExploding = true;

        enemy.enemyStats.RegisterEnemyDeath(enemy.name);

        SpawnExplosionEffect();
        DealExplosionDamage();
        enemy.SelfDestroy();
    }

    private IEnumerator BlinkRedEffect()
    {
        SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
        Color originalColor = sr.color;

        for (int i = 0; i < 6; i++) 
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnExplosionEffect()
    {
        if (enemy.explosivePrefabs != null)
        {
            Vector3 spawnPosition = enemy.attackCheck != null ? enemy.attackCheck.position : enemy.transform.position;
            GameObject explosion = GameObject.Instantiate(enemy.explosivePrefabs, spawnPosition, Quaternion.identity);
            AudioManager.instance.PlaySFX(41,enemy.transform);
            explosion.transform.localScale *= 2.5f;
        }
    }


    private void DealExplosionDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.attackCheckRadius * 3);
        foreach (var hit in colliders)
        {
            CharacterStats targetStats = hit.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(enemy.transform);
                enemy.chacracterStats.DealTotalDamage(targetStats, false);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
        enemy.chacracterStats.MakeInvincible(false);
    }

}
