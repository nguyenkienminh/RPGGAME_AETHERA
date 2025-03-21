using UnityEngine;

public class Enemy_Skeleton2 : Enemy
{
    #region State
    public Skeleton2IdleState idleState { get; private set; }
    public Skeleton2MoveState moveState { get; private set; }
    public Skeleton2AttackState attackState { get; private set; }
    public Skeleton2StunState stunState { get; private set; }
    public Skeleton2DeadState deadState { get; private set; }
    public Skeleton2BattleState battleState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new Skeleton2IdleState(this,stateMachine,"Idle",this);
        moveState = new Skeleton2MoveState(this, stateMachine, "Move", this);
        attackState = new Skeleton2AttackState(this, stateMachine, "Attack", this);
        stunState = new Skeleton2StunState(this, stateMachine, "Stun", this);
        deadState = new Skeleton2DeadState(this, stateMachine, "Die", this);
        battleState = new Skeleton2BattleState(this, stateMachine, "Battle", this);
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
    protected override void Update()
    {
        base.Update();

    }
}
