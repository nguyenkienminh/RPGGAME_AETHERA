using UnityEngine;

public class Enemy_Wizard2 : Enemy
{
    #region State
    public Wizard2IdleState idleState { get; private set; }
    public Wizard2MoveState moveState { get; private set; }
    public Wizard2AttackState attackState { get; private set; }
    public Wizard2StunState stunState { get; private set; }
    public Wizard2DeadState deadState { get; private set; }
    public Wizard2BattleState battleState { get; private set; }
    #endregion 

    protected override void Awake()
    {
        base.Awake();
        idleState = new Wizard2IdleState(this,stateMachine,"Idle",this);
        moveState = new Wizard2MoveState(this, stateMachine, "Move", this);
        attackState = new Wizard2AttackState(this, stateMachine, "Attack", this);
        stunState = new Wizard2StunState(this, stateMachine, "Stun", this);
        deadState = new Wizard2DeadState(this, stateMachine, "Die", this);
        battleState = new Wizard2BattleState(this, stateMachine, "Battle", this);
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
