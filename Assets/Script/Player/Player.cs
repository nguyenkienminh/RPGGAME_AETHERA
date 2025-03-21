using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("teleport")]
     private GameObject TelePort;
    [Header("Attack details")]
    public float[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    public bool isDead { get; private set; }
    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }

    public PlayerFX fx { get; private set; }
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");

        moveState = new PlayerMoveState(this, stateMachine, "Move");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");

        dashState = new PlayerDashState(this, stateMachine, "Dash");

        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");

        wallJumpState = new PlayerWallJump(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");

        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");

        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");

        climbState = new PlayerClimbState(this,stateMachine, "Climb");
    }
    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        fx = GetComponent<PlayerFX>();

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystalSkill.crystalUnclocked)
        {
            skill.crystalSkill.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!skill.parrySkill.CanUseSkill())
            {
                return;
            }
            Inventory.instance.UseFlaskHeal();
        }

        if(TelePort != null)
        {
            AudioManager.instance.PlaySFX(47, null);
            transform.position = TelePort.GetComponent<DoorMove>().GetPositionTele().position;
        }
    }
    #region teleport
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorMove"))
        {
            TelePort = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("DoorMove"))
        {
            TelePort = null;
        }
    }
    #endregion
    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        animator.speed = animator.speed * (1 - _slowPercent);

        Invoke("ReturnDefaultSpeedAnimator", _slowDuration);
    }

    public override void ReturnDefaultSpeedAnimator()
    {
        base.ReturnDefaultSpeedAnimator();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void ExitBlackHoleAbility()
    {
        stateMachine.ChangeState(airState);
    }
    public IEnumerator BusyFor(float second)
    {
        isBusy = true;
        yield return new WaitForSeconds(second);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (isWallDetected())
            return;

        if (!skill.dashSkill.dashUnclocked) return;


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir != 0)
            {
                stateMachine.ChangeState(dashState);
            }
        }
    }
    public override void Die()
    {
        base.Die();

        isDead = true;

        stateMachine.ChangeState(deadState);
    }
    public bool isLadderDetected()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ladder"));
    }

    #region check NPC
    public bool IsNearNPC()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("NPC") || collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
            {
                return true;
            }
        }
        return false;
    }


    #endregion


    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
