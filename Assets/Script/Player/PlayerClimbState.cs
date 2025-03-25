using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private float climbSpeed = 5f; 
    private bool isClimbing;

    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(46, null);
        player.rb.gravityScale = 0; 
        player.rb.linearVelocity = Vector2.zero;
        isClimbing = true;
    }

    public override void Update()
    {
        base.Update();

        float verticalInput = Input.GetAxisRaw("Vertical"); 

        if (isClimbing)
        {
            player.rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);

            if (verticalInput == 0 || !player.isLadderDetected())
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = 4f; 
        isClimbing = false;
    }
}
