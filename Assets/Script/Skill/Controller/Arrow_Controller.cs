using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    [SerializeField] private ParticleSystem arrowTrail;
    private CharacterStats myStats;
    private int facingDirection = 1;

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

        if(facingDirection == 1 && rb.linearVelocity.x < 0)
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
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage((int)damage);

            myStats.DealTotalDamage(collision.GetComponent<CharacterStats>(),false);

            StuckInfo(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInfo(collision);
        }
    }

    private void StuckInfo(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        AudioManager.instance.PlaySFX(0, null);
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
}
