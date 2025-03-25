using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public CharacterStats chacracterStats { get; private set; }
    #endregion
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7,12);
    [SerializeField] protected Vector2 knockbackOffSet = new Vector2(.5f,2);
    [SerializeField] protected float knockbackDuration = .07f;
    protected bool isKnocked;


    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.31f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir {  get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        chacracterStats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercent, float _slowDuration)
    {

    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = 1;
        }else if(_damageDirection.position.x < transform.position.x)
        {
            knockbackDir = -1;
        }
    }
    public virtual void ReturnDefaultSpeedAnimator()
    {
        animator.speed = 1;
    }
    protected virtual void SetupZeroKnockbackPower()
    {

    }

    public void SetupKnockbackPower(Vector2 _knockbackPower)
    {
        knockbackPower = _knockbackPower;
    }
    public virtual void DamageImpact() => StartCoroutine("HitKnockback");

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        float xOffset = Random.Range(knockbackOffSet.x, knockbackOffSet.y);
        //if(knockbackPower.x > 0 || knockbackPower.y > 0) // this line make player immune to freeze effect when he takes it 
        rb.linearVelocity = new Vector2((knockbackPower.x + xOffset) * -knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    #region Collision
    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position
            .x + wallCheckDistance * facingDir, wallCheck.position.y));

        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
            onFlipped();
        }
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }
    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if(facingDir == -1)
        {
            facingRight = false;
        }
    }

    #endregion

    #region Velocity
    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        rb.linearVelocity = new Vector2(0, 0);

    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

   
    public virtual void Die()
    {

    }

    
}
