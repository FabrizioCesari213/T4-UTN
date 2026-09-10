using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{      
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundAcceleration = 20f;
    [SerializeField] private float groundDeceleration = 25f;
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float maxMomentumSpeed = 14f;
    [SerializeField] private float momentumDecay = 1.5f;
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
                float currentSpeed = horizontalVelocity.magnitude;
                if (currentSpeed <= moveSpeed)
                {
                    horizontalVelocity = Vector3.MoveTowards(
                        horizontalVelocity,
                        desiredVelocity,
                        groundAcceleration * Time.deltaTime
                    );
                }
                else
                {
                    Vector3 momentumTarget = moveDirection * currentSpeed; 
                    
                    horizontalVelocity = Vector3.MoveTowards(
                        horizontalVelocity,
                        momentumTarget,
                        groundAcceleration * Time.deltaTime
                    );
                
                if(horizontalVelocity.magnitude > moveSpeed)
                {
                    float decayedSpeed = Mathf.MoveTowards(
                        horizontalVelocity.magnitude,
                        moveSpeed,
                        momentumDecay * Time.deltaTime
                    );
                    horizontalVelocity = horizontalVelocity.normalized * decayedSpeed;
                }
                }
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
       else if (hasMovementInput)
        {
            float currentSpeed = horizontalVelocity.magnitude;

            Vector3 targetVelocity =
                moveDirection * Mathf.Max(currentSpeed, moveSpeed);

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                targetVelocity,
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
