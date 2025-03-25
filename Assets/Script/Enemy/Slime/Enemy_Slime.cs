using NUnit.Framework.Interfaces;
using UnityEngine;


public enum SlimeType
{
    big,
    medium,
    small
}
public class Enemy_Slime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefabs;
    [SerializeField] private Vector2 minCreateVelocity;
    [SerializeField] private Vector2 maxCreateVelocity;

    #region States
    public slimeIdleState idleState {  get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public slimeBattleState battleState { get; private set; }   
    public slimeAttackState attackState { get; private set; }
    public slimStunnedState stunnedState { get; private set; }
    public slimeDeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new slimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new slimeBattleState(this, stateMachine, "Move", this);
        attackState = new slimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new slimStunnedState(this,stateMachine, "Stun", this);
        deadState = new slimeDeadState(this, stateMachine, "Die", this);
    }
    #endregion
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
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

        if(slimeType == SlimeType.small)
        {
            return;
        }

        CreateSlimes(slimeToCreate, slimePrefabs);
    }

    private void CreateSlimes(int _amountOfSlime,GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlime; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDir);            
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if(_facingDir != facingDir)
        {
            Flip();
        }
        float xVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.y);
        float yVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.y);
        isKnocked = true;
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * -facingDir, yVelocity);
        Invoke("CancelKnockBack", 1.5f);
    }

    private void CancelKnockBack() => isKnocked = false;
}
