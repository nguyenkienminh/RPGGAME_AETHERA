using UnityEngine;

public class Enemy_ParentDemon : Enemy
{
    [SerializeField] private GameObject fireCarpetPrefabs;
    public float lastTimeFireCarpet;
    [SerializeField] private float throwFireCarpetCooldown;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] public float waitFireCarpet;
    private Transform player;
    private int fireCarpetCombo;
    #region state
    public ParentDemoIdleState idleState { get; private set; }
    public ParentDemoMoveState moveState { get; private set; }
    public ParentDemoAttackState attackState { get; private set; }
    public ParentDemoDeadState deadState { get; private set; }
    public ParentDemoBattleState battleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ParentDemoIdleState(this, stateMachine, "Idle", this);
        moveState = new ParentDemoMoveState(this, stateMachine, "Move", this);
        attackState = new ParentDemoAttackState(this, stateMachine, "Attack", this);
        deadState = new ParentDemoDeadState(this, stateMachine, "Die", this);
        battleState = new ParentDemoBattleState(this, stateMachine, "Battle", this);
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
    private Vector3 spellPosition;
    private GameObject newFireCarpet;

    public void enemyCaFireCarpet()
    {
        fireCarpetCombo = Random.Range(0,2);


        if (fireCarpetCombo == 0)
        {
            spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset);

            newFireCarpet = Instantiate(fireCarpetPrefabs, spellPosition, Quaternion.identity);
        }
        else if (fireCarpetCombo == 1)
        {

            spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset);
            newFireCarpet = Instantiate(fireCarpetPrefabs, spellPosition, Quaternion.identity);

        }

        newFireCarpet.GetComponent<Animator>().SetFloat("fireCarpetCombo", fireCarpetCombo);

        if (facingDir == 1)
        {
            newFireCarpet.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (facingDir == -1)
        {
            newFireCarpet.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        newFireCarpet.GetComponent<FireCarpet_Controller>().SetupFireCarpet(chacracterStats, facingDir, fireCarpetCombo);
    }
    public bool CanFireCarpet()
    {
        if (Time.time >= lastTimeFireCarpet + throwFireCarpetCooldown)
        {
            return true;
        }
        return false;
    }
}
