using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target == null)
                {
                    return;
                }

                if (_target != null)
                {
                    player.chacracterStats.DealTotalDamage(_target, false);
                }

                ItemData_Equipment weapon = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weapon != null)
                {
                    weapon.Effect(_target.transform);
                }

            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
