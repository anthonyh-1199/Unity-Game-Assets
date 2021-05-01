using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateRangeAttack : EnemyState
{

    int attackTime;

    public override void EnterState(EnemyStateMachine enemy)
    {
        //Set attack timer
        attackTime = 30;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        attackTime--;

        //Stop moving
        enemy.navMeshAgent.velocity = new Vector3(0, 0, 0);

        //Do attack
        if (attackTime == 0){
            Attack(enemy);
            enemy.ChangeState(enemy.STATE_CHASE);
        }

        enemy.debugState = "ATTACK";
    }

    //Do ranged attack
    private void Attack(EnemyStateMachine enemy){
        //Create a projectile object
        ScriptProjectile projectile = GameObject.Instantiate(enemy.projectile, enemy.transform.position + enemy.transform.forward, Quaternion.identity).GetComponent<ScriptProjectile>();;

        //Set origin of projectile to self
        projectile.SetOrigin(enemy.gameObject);

        //Set direction of projectile to target
        Vector3 projectileDirection = enemy.target.transform.position - enemy.transform.position;
        projectile.transform.forward = projectileDirection;

        //Play attack sound
        enemy.PlaySound(enemy.audioClips[5]);
    }
}