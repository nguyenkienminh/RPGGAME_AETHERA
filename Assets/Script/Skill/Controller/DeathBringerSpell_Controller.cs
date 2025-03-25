using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;


    private CharacterStats myStats;

    public void SetupSpell(CharacterStats stats)
    {
        myStats = stats;
    }   
    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0,whatIsPlayer);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DealTotalDamage(hit.GetComponent<CharacterStats>(),true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(check.position, boxSize);
    }

    private void SelfDestroy() => Destroy(gameObject);

}
