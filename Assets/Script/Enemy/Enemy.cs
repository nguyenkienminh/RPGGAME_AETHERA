using System;
using System.Collections;
using System.Xml;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stuned info")]
    public float stunDuration =1;
    public Vector2 stunDirection = new Vector2(7,12);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed = 2f;
    public float idleTime = 2f;
    public float battleTime = 7f;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float arrowDistance = 2;
    public float attackDistance = 3;
    public float attackCooldown;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown = 2;

    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }

    public event Action OnDeath;

    public string lastAnimBoolName {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFX>();

    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

   }

    public virtual void AssignLastAnimName(string name)
    {
        lastAnimBoolName = name;
    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
          moveSpeed = moveSpeed * (1 - _slowPercent);
          animator.speed = animator.speed * (1 - _slowPercent);

          Invoke("ReturnDefaultSpeedAnimator", _slowDuration);
    }
    public override void ReturnDefaultSpeedAnimator()
    {
        base.ReturnDefaultSpeedAnimator();

        moveSpeed = defaultMoveSpeed;
    }
    public virtual void FreezeTimer(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _second) => StartCoroutine(FreezeTimerCoroutine(_second));


    protected virtual IEnumerator FreezeTimerCoroutine(float _second)
    {
        FreezeTimer(true);

        yield return new WaitForSeconds(_second);

        FreezeTimer(false);
    }

    #region counter attack
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;

        counterImage.SetActive(true);

    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);  
    }
    #endregion
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D isPlayerDetected()
    {
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsGround);

        if (wallDetected)
        {
            if (wallDetected.distance < playerDetected.distance)
            {
                return default(RaycastHit2D);
            }
        }
        return playerDetected;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.yellow;    

        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    //public override void Die()
    //{
    //    base.Die();
    //}

    public override void Die()
    {
        base.Die();

        OnDeath?.Invoke(); 
    }

   
}
