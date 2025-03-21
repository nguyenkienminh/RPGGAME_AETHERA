using UnityEngine;

public class Enemy_Skeleton1 : Enemy
{
    #region State
    public Skeleton1IdleState idleState { get; private set; }
    public Skeleton1MoveState moveState { get; private set; }
    public Skeleton1BattleState battleState { get; private set; }
    public Skeleton1AttackState attackState { get; private set; }
    public Skeleton1StunState stunState { get; private set; }
    public Skeleton1DeadState deadState { get; private set; }

    #endregion 

    protected override void Awake()
    {
        base.Awake();
        idleState = new Skeleton1IdleState(this, stateMachine, "Idle", this);
        moveState = new Skeleton1MoveState(this, stateMachine, "Move", this);
        battleState = new Skeleton1BattleState(this, stateMachine, "Battle", this);
        attackState = new Skeleton1AttackState(this, stateMachine, "Attack", this);
        stunState = new Skeleton1StunState(this, stateMachine, "Stun", this);
        deadState = new Skeleton1DeadState(this, stateMachine, "Die", this);
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
