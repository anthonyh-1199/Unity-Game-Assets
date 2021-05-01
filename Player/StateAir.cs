using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAir : PlayerState
{
    //This is the state for when the player is not grounded

    public override void EnterState(PlayerStateMachine player)
    {

    }


    public override void Update(PlayerStateMachine player)
    {
        MoveAir(player);

        CheckTransitions(player);

        player.debugState = "AIR";
    }


    //Checks to see if we need to change to another state
    public void CheckTransitions(PlayerStateMachine player)
    {
        //Transition to StateWalk if the player is touching the ground
        if (player.isGrounded()){
            player.ChangeState(player.STATE_WALK);
        }

        //Transition to StateClimb if the player is falling, moving forwards, and is against a ledge
        if (player.playerVelocity.y < 0){
            if (Input.GetAxisRaw("Vertical") == 1){
                Vector3 horizontalRayPosition = player.transform.position;
                Vector3 verticalRayPosition = player.transform.position;

                horizontalRayPosition.y += 0.70f; //Make point 0.75f
                verticalRayPosition += player.transform.forward;
                verticalRayPosition.y += 1.0f;

                RaycastHit horizontalHit;
                RaycastHit verticalHit;
                if (Physics.Raycast(horizontalRayPosition, player.transform.forward, out horizontalHit, 1.0f, LayerMask.GetMask("Ground"))
                && Physics.Raycast(verticalRayPosition, -player.transform.up, out verticalHit, 1.0f, LayerMask.GetMask("Ground"))){
                    if (horizontalHit.collider.tag == "Wall" && verticalHit.collider.tag == "Wall"){
                        //Check if there's anything blocking the player's way
                        Vector3 hitboxPosition = player.transform.position;
                        hitboxPosition.y = player.transform.position.y + (0.50f);
                        if (!Physics.CheckSphere(hitboxPosition + player.transform.up, 0.50f, LayerMask.GetMask("Ground"))
                        && !Physics.CheckSphere(hitboxPosition + player.transform.up + player.transform.forward, 0.50f, LayerMask.GetMask("Ground"))){
                            player.ChangeState(player.STATE_CLIMB);
                        }
                    }
                }

            }
        }

        //Transition to StateDead is the player's health is 0 or less
        if (player.playerHealth <= 0){
            player.ChangeState(player.STATE_DEAD);
        }
    }


    //Airborne movement
    public void MoveAir(PlayerStateMachine player)
    {
        //Initialize variables
        Vector3 movementVector;

        //Get player input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Horizontal-plane movement vector
        movementVector = (player.transform.right * horizontalInput + player.transform.forward * verticalInput).normalized * player.playerSpeed;

        //Gravity
        player.playerVelocity.y -= player.playerGravity * Time.deltaTime;

        //Move the player in the direction of their Vector3's
        player.CharacterController.Move(movementVector * Time.deltaTime);
        player.CharacterController.Move(player.playerVelocity * Time.deltaTime);

        //Reset vertical velocity when you bump into the ceiling
        if (player.playerVelocity.y > 0){
            Vector3 hitboxPosition = player.transform.position;
            hitboxPosition.y = player.transform.position.y + (0.59f);
            if (Physics.CheckSphere(hitboxPosition, 0.50f, LayerMask.GetMask("Ground"))){
                player.playerVelocity.y = 0;
            }
        }
    }
}
