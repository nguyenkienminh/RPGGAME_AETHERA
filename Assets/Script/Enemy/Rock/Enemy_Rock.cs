using UnityEngine;

public class Enemy_Rock : Enemy
{
    public GameObject prefabsDead;
    public EnemyRockIdleState idleState { get; private set; }
    public EnemyRockDeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyRockIdleState(this, stateMachine, "Idle", this);

        deadState = new EnemyRockDeadState(this, stateMachine, "Die", this);
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

    public void DestroyPrefabs()
    {
        if (prefabsDead != null)
        {
            GameObject.Destroy(prefabsDead);
            prefabsDead = null; 
        }
    }

}
