using UnityEngine;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dashSkill.CloneOnDash();
        stateTimer = player.dashDuration;
        AudioManager.instance.PlaySFX(48, player.transform);
        player.chacracterStats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.chacracterStats.MakeInvincible(false);

    }

    public override void Update()
    {
        base.Update();

        if(!player.isGroundDetected() && player.isWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.fx.CreateAfterImage();
    }
}
