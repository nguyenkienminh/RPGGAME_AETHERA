﻿using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.swordSkill.DotsActive(true);
        //AudioManager.instance.PlaySFX(27, null);


    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);

    }

    public override void Update()
    {
        base.Update();
        
        player.SetZeroVelocity();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            player.skill.swordSkill.DotsActive(false);
            stateMachine.ChangeState(player.idleState);
        }
      
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }
}
