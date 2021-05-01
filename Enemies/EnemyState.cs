using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{

    public abstract void EnterState(EnemyStateMachine stateMachine);

    public abstract void Update(EnemyStateMachine stateMachine);
        
}
