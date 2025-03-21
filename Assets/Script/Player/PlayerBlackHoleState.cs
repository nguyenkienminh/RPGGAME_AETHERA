using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;

    private float defaultGratity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGratity = player.rb.gravityScale;
        rb.gravityScale = 0;

        skillUsed = false;
        stateTimer = flyTime;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGratity;
        player.fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 8);
        }

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
                if (player.skill.BlackholeSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        // exist state blackhole 
        if (player.skill.BlackholeSkill.BlackholeFinished())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
