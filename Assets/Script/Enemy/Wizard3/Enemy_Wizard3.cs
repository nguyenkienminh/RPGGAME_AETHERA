using NUnit.Framework.Interfaces;
using UnityEngine;

public class Enemy_Wizard3 : Enemy
{
    private Transform player;
    [SerializeField] private GameObject exploreWizard3Prefabs;
    [SerializeField] private float yOffsetSpawn;
    [SerializeField] private float xOffsetSpawn;
    [SerializeField] public float waitSecondToSpawn;
    #region States
    public Wizard3IdleState idleState { get; private set; }
    public Wizard3MoveState moveState { get; private set; }
    public Wizard3BattleState battleState { get; private set; }
    public Wizard3AttackState attackState { get; private set; }
    public Wizard3DeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Wizard3IdleState(this, stateMachine, "Idle", this);
        moveState = new Wizard3MoveState(this, stateMachine, "Move", this);
        battleState = new Wizard3BattleState(this, stateMachine, "Battle", this);
        attackState = new Wizard3AttackState(this, stateMachine, "Attack", this);
        deadState = new Wizard3DeadState(this, stateMachine, "Die", this);
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

    public void CanExploreWizard3()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffsetSpawn, transform.position.y + yOffsetSpawn); 
        GameObject newThrowBall = Instantiate(exploreWizard3Prefabs, spawnPosition, Quaternion.identity);

        newThrowBall.GetComponent<Wizard3Explore_Controller>().SetupSpell(chacracterStats);
        newThrowBall.transform.localScale = Vector3.one * 2.6f;

        // Gọi phương thức để di chuyển quả cầu phép về phía player
        newThrowBall.GetComponent<Wizard3Explore_Controller>().MoveTowards(player.position);
    }

}
