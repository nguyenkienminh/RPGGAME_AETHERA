using Assets.Script.Enemy.Archer;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer specify info")]
    [SerializeField] private GameObject arrowPrefabs;
    [SerializeField] private float arrowSpeed;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Addition collison chehck")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    #region state
    public ArcherAttackState attackState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherStunState stunState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        deadState = new ArcherDeadState(this, stateMachine, "Die", this);
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        stunState = new ArcherStunState(this, stateMachine, "Stun", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        //CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();

        //if (collider != null)
        //{
        //    rb.linearVelocity = new Vector2(0, 25);
        //    collider.enabled = false;
        //}
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefabs, attackCheck.position, Quaternion.identity);

        newArrow.GetComponent<Arrow_Controller>().SetupArrow(arrowSpeed * facingDir, chacracterStats);
    }

    public bool GroundBehindCheck()
    {
       return Physics2D.BoxCast(groundBehindCheck.position,groundBehindCheckSize,0,Vector2.zero,0,whatIsGround);
    }

    public bool WallBehinCheck()
    {
        return Physics2D.Raycast(wallCheck.position,Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawCube(groundBehindCheck.position,groundBehindCheckSize);
    }
}
