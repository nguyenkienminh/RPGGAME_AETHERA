using System.Collections;
using UnityEngine;


public class EnemyRockDeadState : EnemyState
{
    Enemy_Rock enemy;
    [SerializeField]
    public EnemyRockDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Rock _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        if (enemy.prefabsDead != null)
        {
            enemy.prefabsDead = GameObject.Instantiate(enemy.prefabsDead, enemy.transform.position, Quaternion.identity);
        }
    }


    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        DestroyRock();
        enemy.DestroyPrefabs();
    }

    public void DestroyRock()
    {
        GameObject.Destroy(enemy.gameObject);
    }
}
