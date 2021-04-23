using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDead : PlayerState
{
    //This is the state for when the player's health is 0 or less
    
    public override void EnterState(PlayerStateMachine player)
    {
        player.debugState = "DEAD";
    }

    public override void Update(PlayerStateMachine player)
    {
        
    }

}
