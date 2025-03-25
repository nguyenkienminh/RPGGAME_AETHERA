using UnityEngine;


[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)] 
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _EnemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = (int)(playerStats.GetMaxValueHP() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
