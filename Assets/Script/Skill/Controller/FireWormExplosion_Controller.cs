using UnityEngine;

public class FireWormExplosion_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float speed = 10f;
    private CharacterStats myStats;
    private int facingDir;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetupSpell(CharacterStats stats, int _facingDir)
    {
        myStats = stats;
        facingDir = _facingDir;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * facingDir * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Ground")
            {
                animator.SetTrigger("Explosion");
                SelfDestroy();

            }
            if (collision.gameObject.tag == "Player")
            {

                Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0, whatIsPlayer);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Player>() != null)
                    {
                        hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                        if (myStats != null)
                        {
                            myStats.DealTotalDamage(hit.GetComponent<CharacterStats>(), false);
                        }


                    }
                }
                AudioManager.instance.PlaySFX(43, null);

                animator.SetTrigger("Explosion");
                SelfDestroy();
            }
        }

    }
    private void OnDrawGizmos()
    {
        if (check != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(check.position, boxSize);
        }


    }

    private void SelfDestroy()
    {
        Destroy(gameObject, 0.1f);
    }
}
