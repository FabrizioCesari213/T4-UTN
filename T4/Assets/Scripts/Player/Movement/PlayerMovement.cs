using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{      
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;


    public void OnMove(InputAction.CallbackContext context){
         moveInput = context.ReadValue<Vector2>();

        
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


    
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        
    }

    
}
