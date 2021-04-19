using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdvancedPlayerMovement : MonoBehaviour
{

    public float playerSpeed = 6;
    public float playerSprintSpeed = 10;
    public float playerGravity = 20;
    public float playerJumpSpeed = 1.1f;

    [SerializeField] bool grounded;

    [SerializeField] private Vector3 playerVelocity;
    
    private CharacterController playerController;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        //Initialize variables
        Vector3 movementVector;

        //Get player input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);


        //Crouching (need to fix)
        if (Input.GetKey(KeyCode.LeftControl)){
            float previousHeight = playerController.height;
            playerController.height = Mathf.Lerp(playerController.height, 1.0f, 20 * Time.deltaTime);

            //Vector3 v = new Vector3(transform.position.x, (playerController.height - previousHeight) / 2, transform.position.z);
            //transform.position -= v;

        } else {
            playerController.height = Mathf.Lerp(playerController.height, 2.0f, 20 * Time.deltaTime);
        }

        //Sprinting and non-sprinting movement
        //Check if the player is 1. Holding down the Sprint key and 2. Moving forwards
        if (isSprinting && verticalInput == 1){
            //Convert input into a normalized Vector3
            movementVector = (transform.right * horizontalInput + transform.forward * verticalInput).normalized * playerSprintSpeed;
        } else {
            //Convert input into a normalized Vector3
            movementVector = (transform.right * horizontalInput + transform.forward * verticalInput).normalized * playerSpeed;
        }

        //Move the player in the direction of the Vector3
        playerController.Move(movementVector * Time.deltaTime);

        //Gravity
        if (isGrounded() && playerVelocity.y < 0){
            playerVelocity.y = -1f;
        }

        playerVelocity.y -= playerGravity * Time.deltaTime;

        //Jumping
        Jump();

        //Move the player up or down depending on their y-value
        playerController.Move(playerVelocity * Time.deltaTime);

        grounded = isGrounded();

    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump")){
            //Check if player is on ground and not moving upwards
            if (isGrounded() && !(playerController.velocity.y > 0)){
                playerVelocity.y += Mathf.Sqrt(playerJumpSpeed * 2f * playerGravity);
            }
        }
    }

    private bool isGrounded()
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
    }
}