using System.Collections;
using UnityEngine;

public class Enemy_FlyEye : Enemy
{
    private Transform player;
    private Vector2 originalPosition;

    #region State
    public FlyEyeIdleState idleState { get; private set; }
    public FlyEyeMoveState moveState { get; private set; }
    public FlyEyeAttackState attackState { get; private set; }
    public FlyEyeBattleState battleState { get; private set; }
    public FlyEyeDeadState deadState { get; private set; }
    #endregion 

    protected override void Awake()
    {
        base.Awake();

        idleState = new FlyEyeIdleState(this, stateMachine, "Idle", this);
        moveState = new FlyEyeMoveState(this, stateMachine, "Move", this);
        attackState = new FlyEyeAttackState(this, stateMachine, "Attack", this);
        battleState = new FlyEyeBattleState(this, stateMachine, "Battle", this);
        deadState = new FlyEyeDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        player = PlayerManager.instance.player.transform;
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

    public void RushAndAttackPlayer()
    {
        originalPosition = transform.position;
        StartCoroutine(RushAttackAndReturn());
    }

    private IEnumerator RushAttackAndReturn()
    {
        Vector2 targetPosition = player.position;
        float attackSpeed = 10f;
        float returnSpeed = 10f;

        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while ((Vector2)transform.position != originalPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        stateMachine.ChangeState(battleState);
    }
}
