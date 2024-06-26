using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        player.Rb.velocity = new Vector2(0f, 0f);
        player.Anim.SetInteger("State", (int)GameEnum.EPlayerState.idle);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(player.DirX != 0 || player.DirY!=0)
        {
            SwitchState(factory.Run());
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
