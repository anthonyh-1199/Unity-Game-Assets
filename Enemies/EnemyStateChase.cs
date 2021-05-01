using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateChase : EnemyState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.attackTimer = 50;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        //If a target object exists, set the parent's destination to that object
        if (enemy.target != null && enemy.navMeshAgent != null){

            //Set destination to that target
            enemy.destination = enemy.target.transform;

            //Tell the Nav Agent to move to the destination
            Vector3 destinationVector = enemy.destination.transform.position;
            enemy.navMeshAgent.SetDestination(destinationVector);

        } else {
            //If a target object does not exist, reset target threshold and return to idle state
            enemy.targetThreshold = 0;
            enemy.ChangeState(enemy.STATE_IDLE);
        }

        CheckTransitions(enemy);

        enemy.debugState = "CHASE";

        //Decrement timers
        enemy.attackTimer--;
        
        if (enemy.targetThreshold > 0){
            enemy.targetThreshold--;
        }

        if (enemy.reactionTime > 0){
            enemy.reactionTime--;
        }
        
        if (enemy.attackTimer == 0){
            //Melee attack
            enemy.attackTimer = 50;
            if (CheckMeleeAttack(enemy)){
                enemy.ChangeState(enemy.STATE_MELEE_ATTACK);
            }

            //Ranged attack
            else if (CheckRangeAttack(enemy)){
                enemy.ChangeState(enemy.STATE_RANGE_ATTACK);
            }

            enemy.attackTimer = 50;
        }
    }

    //Passive check to see if we need to change to another state
    private void CheckTransitions(EnemyStateMachine enemy)
    {
        //Transition back to StateIdle if you don't have a target
        if (enemy.target == null){
            enemy.ChangeState(enemy.STATE_IDLE);
        }

        //Transition to StateDead is the enemy's health is 0 or less
        if (enemy.enemyHealth <= 0){
            enemy.ChangeState(enemy.STATE_DEAD);
        }
    }



    //Check if target object still exists and is still alive
    private bool CheckTargetStatus(GameObject target){
        return (target != null);
    }

    //Check if target object is visible and within melee range
    private bool CheckMeleeAttack(EnemyStateMachine enemy){

        //Check if we have a melee attack state
        if (!enemy.HasMeleeAttack){
            Debug.Log("We don't have a melee attack");
            return false;
        }

        //Check line of sight
        if (!Physics.Raycast(enemy.transform.position, (enemy.target.transform.position - enemy.transform.position))){
            return false;
        }

        //Check distance
        if (!(Vector3.Distance(enemy.transform.position, enemy.target.transform.position) < enemy.meleeRange)){
            
            return false;
        }
        
        return true;
    }

    //Check if target object is visible and within distance
    private bool CheckRangeAttack(EnemyStateMachine enemy){
        //If we do not have line of sight, do not attack
        if (!Physics.Raycast(enemy.transform.position, (enemy.target.transform.position - enemy.transform.position))){
            return false;
        }

        //If our reaction time is not zero, do not attack
        if (enemy.reactionTime != 0){
            return false;
        }

        //Calculate distance to target
        float distance = Vector3.Distance(enemy.transform.position, enemy.target.transform.position) - 2.0f;

        //Cap distance
        if (distance > 8.0f){
            distance = 8.0f;
        }

        //If we do not have a melee attack, decrease distance threshold
        if (!enemy.HasMeleeAttack){
            distance -= 4.0f;
        }

        //Get random threshold - the closer we are, the more likely we attack
        float f = Random.Range(1.0f, 8.0f);
        Debug.Log(f + " versus: " + distance);

        if (Random.Range(1.0f, 8.0f) < distance){
            return false;
        }

        return true;
    }
}