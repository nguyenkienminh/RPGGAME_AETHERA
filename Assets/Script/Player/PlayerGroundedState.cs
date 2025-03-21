using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.BlackholeSkill.blackHoleUnclocked)
        {
            if (player.skill.BlackholeSkill.cooldownTimer > 0)
            {
                player.fx.CreatePopupText("Cooldown!");
                return;
            }
            stateMachine.ChangeState(player.blackHoleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.swordSkill.swordUnclocked)
        {
            //if (!player.skill.parrySkill.CanUseSkill())
            //{
            //    return;
            //}
            if (!player.skill.swordSkill.CanUseSkill())
            {
                return;
            }
            stateMachine.ChangeState(player.aimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parrySkill.parryUnclocked)
        {
            if (!player.skill.parrySkill.CanUseSkill()) 
            {
                return; 
            }
            stateMachine.ChangeState(player.counterAttack);
            return;
        }
        if (Input.GetKeyDown(KeyCode.W) && player.isLadderDetected())
        {
            stateMachine.ChangeState(player.climbState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (!player.isGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

        return false;
    }
}
