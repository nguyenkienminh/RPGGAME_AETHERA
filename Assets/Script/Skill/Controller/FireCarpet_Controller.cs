using UnityEngine;

public class FireCarpet_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    private CharacterStats myStats;
    [SerializeField] private float speed = 10f;
    private int facingDir;
    [SerializeField] private GameObject explosionPrefabs;
    private int fireCarpetCombo;
    public void SetupFireCarpet(CharacterStats stats, int? _facingDir,int _fireCarpetCombo)
    {
        facingDir = (int)_facingDir;
        myStats = stats;
        fireCarpetCombo = _fireCarpetCombo;
    }
    private void Update()
    {
        transform.Translate(Vector2.right * speed * facingDir * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
         
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
                            myStats.DealTotalDamage(hit.GetComponent<CharacterStats>(), true);
                        }


                    }
                }        
            }

            if (explosionPrefabs != null && fireCarpetCombo == 1)
            {
                GameObject newExplore = Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
                AudioManager.instance.PlaySFX(43, null);
                newExplore.transform.localScale *= 1.3f;
                Destroy(newExplore, .5f);
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
        Destroy(gameObject);
    }
}
