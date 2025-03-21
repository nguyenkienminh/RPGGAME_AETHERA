using UnityEngine;

public class PlayerWallJump : PlayerState
{
    public PlayerWallJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = .4f;
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(player.airState);

        if (player.isGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }


   
}
