using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{      
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundAcceleration = 20f;
    [SerializeField] private float groundDeceleration = 25f;
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 2f;
    private Vector2 moveInput;
    private CharacterController controller;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    public void OnMove(InputAction.CallbackContext context){
         moveInput = context.ReadValue<Vector2>();

        
    }
   public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {   
      

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
        forward * moveInput.y +
        right * moveInput.x;

        moveDirection = Vector3.ClampMagnitude(
            moveDirection,
            1f
            );
        Vector3 desiredVelocity = moveDirection * moveSpeed;
        bool hasMovementInput = moveInput.sqrMagnitude > 0.01f;
    if(controller.isGrounded){
        if (hasMovementInput)
            {
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    desiredVelocity,
                    groundAcceleration * Time.deltaTime
                );
            }
            else
            {
                horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                groundDeceleration * Time.deltaTime
                );
            }  
       }
       else
        {
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                desiredVelocity,
                airAcceleration * Time.deltaTime
            );
        }


         if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = horizontalVelocity;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
        
    }

    
}
