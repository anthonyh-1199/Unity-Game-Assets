using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCrouch : PlayerState
{
    //This is the state for when the player is grounded and not touching any movement keys

    public override void EnterState(PlayerStateMachine player)
    {
        player.playerSpeed = player.playerWalkSpeed;
    }


    public override void Update(PlayerStateMachine player)
    {
        Move(player);

        CheckTransitions(player);

        player.debugState = "CROUCH";
    }


    //Checks to see if we need to change to another state
    public void CheckTransitions(PlayerStateMachine player)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Check if the player is not holding down the crouch button
        if (!Input.GetKey(KeyCode.LeftControl)){
            //Check if the player has enough room to stand up
            if (CanStand(player)){
                //Transition to StateIdle if the player is not pressing the movement keys
                if (horizontalInput == 0 && verticalInput == 0){
                    player.ChangeState(player.STATE_IDLE);
                } else {
                    player.ChangeState(player.STATE_WALK);
                }
            }
        }

        //Transition to StateAir if the player is not grounded
        if (!player.isGrounded()){
            player.ChangeState(player.STATE_AIR);
        }

        //Transition to StateDead is the player's health is 0 or less
        if (player.playerHealth <= 0){
            player.ChangeState(player.STATE_DEAD);
        }
    }


    public void Move(PlayerStateMachine player)
    {
        //Initialize variables
        Vector3 movementVector;

        //Get player input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Convert input into a normalized vector
        movementVector = (player.transform.right * horizontalInput + player.transform.forward * verticalInput).normalized * player.playerCrouchSpeed;

        //Base gravity
        if (player.playerVelocity.y <= 0){
            player.playerVelocity.y = -1f;
        }
        player.playerVelocity.y -= player.playerGravity * Time.deltaTime;

        //Move the player in the direction of their Vector3's
        player.CharacterController.Move(movementVector * Time.deltaTime);
        player.CharacterController.Move(player.playerVelocity * Time.deltaTime);

        //Go into crouching position
        float previousHeight = player.CharacterController.height;
        player.CharacterController.height = Mathf.Lerp(player.CharacterController.height, 1.0f, 20 * Time.deltaTime);
    }


    private bool CanStand(PlayerStateMachine player)
    {
        Vector3 hitboxPosition = player.transform.position;
        hitboxPosition.y = player.transform.position.y + (0.50f);
        return (!Physics.CheckSphere(hitboxPosition, 0.50f, LayerMask.GetMask("Ground")));
    }
}
