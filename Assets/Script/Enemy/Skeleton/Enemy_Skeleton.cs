using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }

    public SkeletonBattleState battleState { get; private set; }

    public SkeletonAttackState attackState { get; private set; }

    public SkeletonStunState stunState { get; private set; }

    public SkeletonDeadState deadState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);

        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);

        battleState = new SkeletonBattleState(this, stateMachine, "Battle", this);

        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);

        stunState = new SkeletonStunState(this, stateMachine, "Stun", this);

        deadState = new SkeletonDeadState(this, stateMachine, "Die", this);
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
    protected override void Update()
    {
        base.Update();

    }


}
