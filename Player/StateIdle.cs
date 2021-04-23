using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : PlayerState
{
    //This is the state for when the player is grounded, not touching any movement keys, and is not crouching

    public override void EnterState(PlayerStateMachine player)
    {

    }

    public override void Update(PlayerStateMachine player)
    {
        Move(player);

        CheckTransitions(player);

        player.debugState = "IDLE";
    }


    public void Move(PlayerStateMachine player)
    {
        //Base gravity
        if (player.playerVelocity.y <= 0){
            player.playerVelocity.y = -1f;
        }
        player.playerVelocity.y -= player.playerGravity * Time.deltaTime;

        //Move the player in the direction of their Vector3
        player.CharacterController.Move(player.playerVelocity * Time.deltaTime);

        //Jumping
        if (Input.GetButtonDown("Jump")){
            //Check if player is on ground and not moving upwards
            if (player.canJump()){
                player.playerVelocity.y += Mathf.Sqrt(player.playerJumpSpeed * 2f * player.playerGravity);
                player.ChangeState(player.STATE_AIR);
            }
        }

        //Returning from crouch
        player.CharacterController.height = Mathf.Lerp(player.CharacterController.height, 2.0f, 20 * Time.deltaTime);
    }


    //Passive check to see if we need to change to another state
    public void CheckTransitions(PlayerStateMachine player)
    {
        //Transition to StateWalk if the player is pressing the movement keys
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            player.ChangeState(player.STATE_WALK);
        }

        //Transition to StateAir if the player is not grounded
        if (!player.isGrounded()){
            player.ChangeState(player.STATE_AIR);
        }

        //Transition to StateCrouch is the player is pressing the crouch button
        if (Input.GetKey(KeyCode.LeftControl)){
            player.ChangeState(player.STATE_CROUCH);
        }

        //Transition to StateDead is the player's health is 0 or less
        if (player.playerHealth <= 0){
            player.ChangeState(player.STATE_DEAD);
        }
    }

}
