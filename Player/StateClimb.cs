using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateClimb : PlayerState
{
    //This is the state for when the player is in the air and moving downwards and forwards against a climbable object

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
