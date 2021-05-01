using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDead : EnemyState
{

    public override void EnterState(EnemyStateMachine enemy)
    {   
        //Set status to dead
        enemy.isDead = true;

        //Play death sound
        enemy.PlaySound(enemy.audioClips[Random.Range(2, 4)]);
    }

    public override void Update(EnemyStateMachine enemy)
    {
        enemy.debugState = "DEAD";
        enemy.Kill();
    }

}
