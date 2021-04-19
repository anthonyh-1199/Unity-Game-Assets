using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{

    public abstract void EnterState(PlayerStateMachine stateMachine);

    public abstract void Update(PlayerStateMachine stateMachine);
        
}
