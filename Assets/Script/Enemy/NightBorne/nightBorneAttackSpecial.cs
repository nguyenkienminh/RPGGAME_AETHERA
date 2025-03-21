using System.Collections;
using UnityEngine;


public class nightBorneAttackSpecial : EnemyState
{
    protected Enemy_NightBorne enemy;

    private int amountOfSpells;
    private float spellTimer;
    public nightBorneAttackSpecial(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells = enemy.amountOfSpells;
        spellTimer = .5f;
        AudioManager.instance.PlaySFX(41, enemy.transform);
    }
    private IEnumerator ThrowBallWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        enemy.enemyCanThrowBall();

    }
    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;


        if (CanThrowBall())
        {
            enemy.StartCoroutine(ThrowBallWithDelay(.1f));
        }

        if (amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;

    }
    private bool CanThrowBall()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
