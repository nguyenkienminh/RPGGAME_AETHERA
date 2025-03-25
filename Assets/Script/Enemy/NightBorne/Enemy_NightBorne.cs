using System.Collections;
using Assets.Script.Enemy.NightBorne;
using UnityEngine;

public class Enemy_NightBorne : Enemy
{
    [SerializeField] private GameObject throwBallPrefabs;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float throwBallStateCooldown;
    [SerializeField] public float delayAudio;
    #region State
    public nightBorneIdleState idleState { get; private set; }
    public nightBorneMoveState moveState { get; private set; }
    public nightBorneBattleState battleState { get; private set; }
    public nightBorneDeadState deadState { get; private set; }
    public nightBorneAttackState attackState { get; private set; }
    public nightBorneStunState stunState { get; private set; }
    public nightBorneAttackSpecial attackSpecialState { get; private set; }
    #endregion 

    protected override void Awake()
    {
        base.Awake();
        idleState = new nightBorneIdleState(this, stateMachine, "Idle", this);
        moveState = new nightBorneMoveState(this, stateMachine, "Move", this);
        battleState = new nightBorneBattleState(this, stateMachine, "Battle", this);
        deadState = new nightBorneDeadState(this, stateMachine, "Die", this);
        attackState = new nightBorneAttackState(this, stateMachine, "Attack", this);
        stunState = new nightBorneStunState(this, stateMachine, "Stun", this);
        attackSpecialState = new nightBorneAttackSpecial(this, stateMachine, "AttackSpecial", this);
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
    public IEnumerator DelayAudio(float second)
    {
        yield return new WaitForSeconds(second);
        AudioManager.instance.PlaySFX(0, null);
    }
    public void enemyCanThrowBall()
    {
        Vector3 spellPosition = new Vector3(transform.position.x + (2 * facingDir), transform.position.y + 0.5f);
        GameObject newThrowBall = Instantiate(throwBallPrefabs, spellPosition, Quaternion.identity);



        newThrowBall.GetComponent<NightBorneThrowBall_Controller>().SetupSpell(chacracterStats, facingDir);
        newThrowBall.transform.localScale = Vector3.one * 0.6f;
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(attackSpecialState);
        }

    }
    public bool CanThrowball()
    {
        if (Time.time >= lastTimeCast + throwBallStateCooldown)
        {
            return true;
        }
        return false;
    }

}
