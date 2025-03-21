﻿using System.Diagnostics;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(14, null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(14);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if (xInput == 0 || player.isWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
