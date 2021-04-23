using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : PlayerState
{   
    //This is the state for when the player is grounded and is not crouching, but is touching the movement keys

    public override void EnterState(PlayerStateMachine player){
        player.playerSpeed = 6;
        player.playerSprintSpeed = 10;
    }


    public override void Update(PlayerStateMachine player){
        //Check state transitions
        CheckTransitions(player);

        //Run movement script
        Move(player);

        player.debugState = "WALK";
    }


    public void Move(PlayerStateMachine player){
        //Initialize variables
        Vector3 movementVector;

        //Get player input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        //Sprinting and non-sprinting movement
        //Check if the player is 1. Holding down the Sprint key and 2. Moving forwards and then convert input into a normalized vector
        if (isSprinting && verticalInput == 1){
            player.playerSpeed = player.playerSprintSpeed;
        } else {
            player.playerSpeed = player.playerWalkSpeed;
        }

        movementVector = (player.transform.right * horizontalInput + player.transform.forward * verticalInput).normalized * player.playerSpeed;

        //Base gravity
        if (player.playerVelocity.y <= 0){
            player.playerVelocity.y = -1f;
        }
        player.playerVelocity.y -= player.playerGravity * Time.deltaTime;

        //Jumping
        if (Input.GetButtonDown("Jump")){
            //Check if player is on ground and not moving upwards
            if (player.canJump()){
                player.playerVelocity.y += Mathf.Sqrt(player.playerJumpSpeed * 2f * player.playerGravity);
                player.ChangeState(player.STATE_AIR);
            }
        }

        //Move the player in the direction of their Vector3's
        player.CharacterController.Move(movementVector * Time.deltaTime);
        player.CharacterController.Move(player.playerVelocity * Time.deltaTime);

        //Returning from crouch
        player.CharacterController.height = Mathf.Lerp(player.CharacterController.height, 2.0f, 20 * Time.deltaTime);
    }
    

    //Checks to see if we need to change to another state
    public void CheckTransitions(PlayerStateMachine player)
    {
        //Transition to StateIdle if the player is not pressing the movement keys
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0){
            player.ChangeState(player.STATE_IDLE);
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
