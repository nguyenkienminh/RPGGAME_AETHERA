using UnityEngine;

public class Enemy_FireWorm : Enemy
{
    [SerializeField] private GameObject MoveAndExplosionFireWormPrefabs;
    public float deplay;
    #region State
    public FireWormIdleState idleState { get; private set; }
    public FireWormMoveState moveState { get; private set; }
    public FireWormDeadState deadState { get; private set; }
    public FireWormAttackState attackState { get; private set; }
    public FireWormBattleState battleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new FireWormIdleState(this,stateMachine,"Idle",this);
        moveState = new FireWormMoveState(this, stateMachine, "Move", this);
        deadState = new FireWormDeadState(this, stateMachine, "Die", this);
        attackState = new FireWormAttackState(this, stateMachine, "Attack", this);
        battleState = new FireWormBattleState(this, stateMachine, "Battle", this);  
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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

    public void enemyCanMoveAndExplosive()
    {
        Vector3 spellPosition = new Vector3(transform.position.x + (2 * facingDir), transform.position.y);
        GameObject newThrowBall = Instantiate(MoveAndExplosionFireWormPrefabs, spellPosition, Quaternion.identity);



        newThrowBall.GetComponent<FireWormExplosion_Controller>().SetupSpell(chacracterStats, facingDir);
        newThrowBall.transform.localScale = Vector3.one * 0.6f;
    }

 
}
