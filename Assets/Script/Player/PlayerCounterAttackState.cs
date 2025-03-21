using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfullCounterAttack", false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessFullCounter();
            }
            if (hit.GetComponent<Arrow2_Controller>() != null)
            {
                hit.GetComponent<Arrow2_Controller>().FlipArrow();
                SuccessFullCounter();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SuccessFullCounter();
                    AudioManager.instance.PlaySFX(51, null);

                    player.skill.parrySkill.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parrySkill.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessFullCounter()
    {
        stateTimer = 10;
        player.animator.SetBool("SuccessfullCounterAttack", true);
    }
}
