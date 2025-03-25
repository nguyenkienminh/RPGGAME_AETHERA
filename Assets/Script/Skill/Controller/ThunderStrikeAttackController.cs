using UnityEngine;

public class ThunderStrikeAttackController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats == null)
            {
                return;
            }

            if (playerStats != null)
            {
                playerStats.DoMagicalDamage(enemyStats);
            }

        }
    }

}
