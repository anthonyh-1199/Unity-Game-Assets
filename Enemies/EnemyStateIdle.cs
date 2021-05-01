using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyState
{

    int viewAngle;
    GameObject player;

    public override void EnterState(EnemyStateMachine enemy)
    {
        viewAngle = 90;
        player = GameObject.Find("Player");
    }

    public override void Update(EnemyStateMachine enemy)
    {
        CheckTransitions(enemy);

        enemy.debugState = "IDLE";
    }

    //Passive check to see if we need to change to another state
    public void CheckTransitions(EnemyStateMachine enemy)
    {
        if (player != null){
            //SIGHT CHECK//
            //Check if the player is within a 180 degree view cone in front of the monster
            if (Vector3.Angle(enemy.transform.forward, player.transform.position - enemy.transform.position) < viewAngle) {

                RaycastHit hit;

                //Check if the monster has direct line of sight with the player
                if (Physics.Raycast(enemy.transform.position, (player.transform.position - enemy.transform.position), out hit)){

                    //Set the target to the player and begin chasing
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
                        enemy.target = hit.collider.gameObject;
                        enemy.PlaySound(enemy.audioClips[Random.Range(0, 2)]);
                        enemy.ChangeState(enemy.STATE_CHASE);
                    }
                }
            }
            //DISTANCE CHECK//
            //Check if the player is within melee range of the monster
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < enemy.meleeRange){

                    //Set the target to the player and begin chasing
                    enemy.target = player;
                    enemy.PlaySound(enemy.audioClips[Random.Range(0, 2)]);
                    enemy.ChangeState(enemy.STATE_CHASE);
            }
        }

        //Transition to StateDead is the enemy's health is 0 or less
        if (enemy.enemyHealth <= 0){
            enemy.ChangeState(enemy.STATE_DEAD);
        }
    }
}
