using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float _lifeTime;
    public PlayerAttackState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        _lifeTime = 0;
        player.Anim.SetInteger("State", (int)GameEnum.EPlayerState.attack);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _lifeTime += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        player.Rb.velocity = new Vector2(player.DirX, player.DirY).normalized * player.MoveSpeed;
    }

    public override void CheckSwitchState()
    {
        if(_lifeTime >= 0.5f)
        {
            SwitchState(factory.Idle());
        }
    }

    public override void ExitState()
    {
        player.vfx.SetActive(false);
    }
}
