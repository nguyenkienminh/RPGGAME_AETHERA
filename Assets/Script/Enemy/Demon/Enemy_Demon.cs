using UnityEngine;

public class Enemy_Demon : Enemy
{
    private Transform player;
    [SerializeField] private GameObject exploreWizard3Prefabs;
    [SerializeField] private float yOffsetSpawn;
    [SerializeField] private float xOffsetSpawn;
    [SerializeField] public float waitSecondToSpawn;

    #region   State
    public DemonIdleState idleState { get; private set; }
    public DemonMoveState moveState { get; private set; }
    public DemonBattleState battleState { get; private set; }
    public DemonAttackState attackState { get; private set; }
    public DemonDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new DemonIdleState(this, stateMachine, "Idle", this);
        moveState = new DemonMoveState(this, stateMachine, "Move", this);
        battleState = new DemonBattleState(this, stateMachine, "Battle", this);
        attackState = new DemonAttackState(this, stateMachine, "Attack", this);
        deadState = new DemonDeadState(this, stateMachine, "Die", this);

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
    public void CanExploreDemon()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffsetSpawn, transform.position.y + yOffsetSpawn);
        GameObject newThrowBall = Instantiate(exploreWizard3Prefabs, spawnPosition, Quaternion.identity);

        newThrowBall.GetComponent<DemonExplore_Controller>().SetupExplore(chacracterStats);
        newThrowBall.transform.localScale = Vector3.one * 2.6f;

        newThrowBall.GetComponent<DemonExplore_Controller>().MoveTowards(player.position);
    }

}
