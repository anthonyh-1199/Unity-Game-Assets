using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateClimb : PlayerState
{

    public override void EnterState(PlayerStateMachine player)
    {
        player.playerVelocity.y = Mathf.Sqrt(player.playerJumpSpeed * 3f * player.playerGravity);
        player.ChangeState(player.STATE_AIR);
    }


    public override void Update(PlayerStateMachine player)
    {
        player.debugState = "CLIMB";
    }


}
