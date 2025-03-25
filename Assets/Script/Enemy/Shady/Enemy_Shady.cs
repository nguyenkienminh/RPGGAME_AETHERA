using Assets.Script.Enemy.Shady;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady Specific")]
    public float battleStateMoveSpeed;

    [SerializeField] public GameObject explosivePrefabs;
    public TextMeshPro boomText;
    public EnemyStats enemyStats { get; private set; }

    #region State
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyStunState stunState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
        stunState = new ShadyStunState(this, stateMachine, "Stun", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
    }

    protected override void Update()
    {
        base.Update();

        if (boomText != null)
        {
            boomText.transform.rotation = Quaternion.identity;
            boomText.transform.position = transform.position + new Vector3(0.5f, 2f, 0);
        }


    }
    protected override void Start()
    {
        base.Start();
        enemyStats = GetComponent<EnemyStats>();

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


    public void SelfDestroy() => Destroy(gameObject); 

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackCheckRadius * 3);
    }
}
