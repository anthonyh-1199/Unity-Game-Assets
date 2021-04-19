using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine : MonoBehaviour
{
    //Initialize varialbes

    //Player movement speeds
    public float playerSpeed;
    public float playerWalkSpeed = 6;
    public float playerCrouchSpeed = 3;
    public float playerSprintSpeed = 10;
    public float playerGravity = 20;
    public float playerJumpSpeed = 1.3f;

    private PlayerState currentState;

    public string debugState;

    //Initialize possible states
    public readonly StateIdle STATE_IDLE = new StateIdle();
    public readonly StateWalk STATE_WALK = new StateWalk();
    public readonly StateAir STATE_AIR = new StateAir();
    public readonly StateCrouch STATE_CROUCH = new StateCrouch();
    public readonly StateClimb STATE_CLIMB = new StateClimb();

    [SerializeField] bool grounded;

    [SerializeField] public Vector3 playerVelocity;
    
    private CharacterController playerController;


    void Start()
    {
        playerController = GetComponent<CharacterController>();
        ChangeState(STATE_IDLE);
        debugState = "IDLE";
        playerSpeed = playerWalkSpeed;
    }


    void Update()
    {
        currentState.Update(this);

        grounded = isGrounded();

        Vector3 horizontalRayPosition = transform.position;
        horizontalRayPosition.y += 0.75f;

        RaycastHit horizontalHit;
        if (Physics.Raycast(horizontalRayPosition, transform.forward, out horizontalHit, 1.0f, LayerMask.GetMask("Ground"))){
            Debug.DrawRay(horizontalRayPosition, transform.forward, Color.red);
        } else {
            Debug.DrawRay(horizontalRayPosition, transform.forward, Color.green);
        }
        

        Vector3 verticalRayPosition = transform.position;
        verticalRayPosition += transform.forward;
        verticalRayPosition.y += 1.0f;
        
        RaycastHit verticalHit;
        if (Physics.Raycast(verticalRayPosition, -transform.up, out verticalHit, 1.0f, LayerMask.GetMask("Ground"))){
            Debug.DrawRay(verticalRayPosition, -transform.up, Color.red);
        } else {
            Debug.DrawRay(verticalRayPosition, -transform.up, Color.blue);
        }
    }


    //Sets the current state to a new state and calls the new state's EnterState method
    public void ChangeState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }


    //Allows state to access our private Character Controller
    public CharacterController CharacterController
    {
        get {return playerController;}
    }


    public bool canJump()
    {
        //Check if player is on ground and not moving upwards
        return (isGrounded() && !(playerController.velocity.y > 0));
    }


    public bool isGrounded()
    {
        Vector3 hitboxPosition = transform.position;
        hitboxPosition.y = transform.position.y - (0.59f * playerController.height / 2.0f);
        return (Physics.CheckSphere(hitboxPosition, 0.50f, LayerMask.GetMask("Ground")));
    }


    void OnDrawGizmos()
    {
        playerController = GetComponent<CharacterController>();
        Vector3 hitboxPosition = transform.position;
        hitboxPosition.y = transform.position.y - (0.59f * playerController.height / 2.0f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitboxPosition, 0.50f);

        hitboxPosition.y = transform.position.y + (0.50f);
        Gizmos.DrawSphere(hitboxPosition, 0.50f);
    }

    
}