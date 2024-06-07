using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        player.Anim.SetInteger("State", (int)GameEnum.EPlayerState.walk);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        player.UpdateObjectDirX();
        if(player.DirY == 0)
        {
            player.Anim.SetInteger("Facing", (int)GameEnum.EPlayerFacing.horizontal);
        }
        else if (player.DirY > 0)
        {
            player.Anim.SetInteger("Facing", (int)GameEnum.EPlayerFacing.back);
        }
        else if(player.DirY < 0 && player.DirX != 0)
        {
            player.Anim.SetInteger("Facing", (int)GameEnum.EPlayerFacing.horizontal);
        }
        else
        {
            player.Anim.SetInteger("Facing", (int)GameEnum.EPlayerFacing.front);
        }
    }

    public override void FixedUpdateState()
    {
        player.Rb.velocity = new Vector2(player.DirX, player.DirY).normalized * player.MoveSpeed;
    }

    public override void CheckSwitchState()
    {
        if(player.DirX == 0 && player.DirY == 0)
        {
            SwitchState(factory.Idle());
        }
        else if(Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            SwitchState(factory.Attack());
        }
    }

    public override void ExitState()
    {

    }
}