using UnityEngine;

public class Arrow2_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;
    [SerializeField] private GameObject prefabsExplosive;
    [SerializeField] private ParticleSystem arrowTrail;
    private CharacterStats myStats;
    private int facingDirection = 1;

    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask whatIsLayer;
    [SerializeField] private float explosionDelay;
    void Start()
    {
        if (arrowTrail != null)
        {
            arrowTrail.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }

        if (facingDirection == 1 && rb.linearVelocity.x < 0)
        {
            facingDirection = -1;
            sr.flipX = true;
        }
    }
    public void SetupArrow(float _speed, CharacterStats _myStats)
    {
        sr = GetComponent<SpriteRenderer>();
        xVelocity = _speed;
        myStats = _myStats;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName) || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                if (myStats != null && collision.GetComponent<CharacterStats>() != null)
                {
                    myStats.DealTotalDamage(collision.GetComponent<CharacterStats>(), false);
                }
                else
                {
                    Debug.LogWarning("myStats hoặc CharacterStats bị null khi mũi tên va chạm.");
                }
            }

            StuckInfo(collision);
            AudioManager.instance.PlaySFX(41,transform);
            Invoke("Explode", explosionDelay);
            SelfDestroy();
        }
    }

    private void Explode()
    {
        GameObject newExplore = Instantiate(prefabsExplosive, transform.position, Quaternion.identity);
        newExplore.transform.localScale *= 3f;
        Destroy(newExplore, .5f);

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsLayer);
        foreach (Collider2D enemy in enemiesHit)
        {
            CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                myStats.DealTotalDamage(enemyStats, false);
            }
        }
    }

    private void StuckInfo(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        Destroy(gameObject, 2);
    }

    public void FlipArrow()
    {
        if (flipped)
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
    private void SelfDestroy()
    {
        Destroy(gameObject, 0.1f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
