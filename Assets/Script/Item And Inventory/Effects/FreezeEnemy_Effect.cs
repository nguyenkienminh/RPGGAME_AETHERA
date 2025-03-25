using UnityEngine;


[CreateAssetMenu(fileName = "Freeze enemy effect", menuName = "Data/Item Effect/Freeze enemy effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;
    public override void ExecuteEffect(Transform _EnemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxValueHP() * .1f) return;

        if(!Inventory.instance.CanUserArmor()) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_EnemyPosition.position, 2);

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.FreezeTimeFor(duration);
            }
        }

    }
}
