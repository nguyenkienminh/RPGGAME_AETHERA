using UnityEngine;

public class Enemy_Archer2 : Enemy
{
    [Header("Archer specify info")]
    [SerializeField] private GameObject arrowPrefabs;
    [SerializeField] private float arrowSpeed;
    public Vector2 jumpVelocity;
    public Vector2 dashVelocity;
    public float jumpCooldown;
    public float DashCooldown;
    [HideInInspector] public float lastTimeJumped;
    [HideInInspector] public float lastTimeDashed;
    public float safeDistance;
    public float delayDash;

    [Header("Addition collison chehck")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    #region State
    public Archer2IdleState idleState { get; private set; }
    public Archer2MoveState moveState { get; private set; }
    public Archer2DeadState deadState { get; private set; }
    public Archer2StunState stunState { get; private set; }
    public Archer2AttackState attackState { get; private set; }
    public Archer2BattleState battleState { get; private set; }
    public Archer2JumpState jumpState { get; private set; }
    public Archer2DashState dashState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Archer2IdleState(this, stateMachine, "Idle", this);
        moveState = new Archer2MoveState(this, stateMachine, "Move", this);
        deadState = new Archer2DeadState(this, stateMachine, "Die", this);
        stunState = new Archer2StunState(this, stateMachine, "Stun", this);
        attackState = new Archer2AttackState(this, stateMachine, "Attack", this);
        battleState = new Archer2BattleState(this, stateMachine, "Idle", this);
        jumpState = new Archer2JumpState(this, stateMachine, "Jump", this);
        dashState = new Archer2DashState(this, stateMachine, "Dash", this);
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
    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefabs, attackCheck.position, Quaternion.identity);

        newArrow.GetComponent<Arrow2_Controller>().SetupArrow(arrowSpeed * facingDir, chacracterStats);
    }

    public bool GroundBehindCheck()
    {
        return Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    public bool WallBehinCheck()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawCube(groundBehindCheck.position, groundBehindCheckSize);
    }
}
