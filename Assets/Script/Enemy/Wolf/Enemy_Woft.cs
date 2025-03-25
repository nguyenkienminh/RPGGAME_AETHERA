using Assets.Script.Enemy.Wolf;
using UnityEngine;

public class Enemy_Woft : Enemy
{
    #region State
    public WolfIdleState idleState { get; private set; }
    public WolfMoveState moveState { get; private set; }
    public WolfDeadState deadState { get; private set; }
    public WolfAttackState attackState { get; private set; }
    public WolfBattleState battleState { get; private set; }
    public WolfStunState stunState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new WolfIdleState(this, stateMachine, "Idle", this);
        moveState = new WolfMoveState(this, stateMachine, "Move", this);
        deadState = new WolfDeadState(this, stateMachine, "Die", this);
        attackState = new WolfAttackState(this, stateMachine, "Attack", this);
        battleState = new WolfBattleState(this, stateMachine, "Battle", this);
        stunState = new WolfStunState(this, stateMachine, "Stun", this);
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
