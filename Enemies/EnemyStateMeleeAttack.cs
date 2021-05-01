using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMeleeAttack : EnemyState
{

    int attackTime;

    public override void EnterState(EnemyStateMachine enemy)
    {
        //Set attack timer
        attackTime = 20;
        Attack(enemy);
    }

    public override void Update(EnemyStateMachine enemy)
    {
        attackTime--;

        //Stop moving
        enemy.navMeshAgent.velocity = new Vector3(0, 0, 0);

        //Do attack
        if (attackTime == 0){
            enemy.ChangeState(enemy.STATE_CHASE);
        }

        enemy.debugState = "ATTACK";
    }

    //Do melee attack
    private void Attack(EnemyStateMachine enemy){

        //Deal damage to target
        if (enemy.target.gameObject.tag == "Enemy"){
            EnemyStateMachine e = enemy.target.GetComponent<EnemyStateMachine>();
            e.TakeDamage(enemy.meleeDamage, "melee", enemy.transform.parent.gameObject);
        }

        //Play attack sound
        enemy.PlaySound(enemy.audioClips[6]);
    }
}