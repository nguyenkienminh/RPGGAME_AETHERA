using UnityEngine;

public class NightBorneThrowBall_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject explosionPrefabs;
    private CharacterStats myStats;
    private int facingDir;
 
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
                GameObject newExplore = Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
                newExplore.transform.localScale *= 1.3f;
                Destroy(newExplore,.5f);
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

                if (explosionPrefabs != null)
                {
                    GameObject newExplore = Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
                    newExplore.transform.localScale *= 1.3f;
                    Destroy(newExplore,.5f);
                    SelfDestroy();
                }
            }
        }

    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DealTotalDamage(hit.GetComponent<CharacterStats>(), false);
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
