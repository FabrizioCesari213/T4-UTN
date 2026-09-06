using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    /*
    void Start()
    {
        
    }
       
    */
    public void OnMove(InputAction.CallbackContext context){
         moveInput = context.ReadValue<Vector2>();

        Debug.Log(moveInput);
    }
    
    void Update()
    {
        Vector3 moveDirection = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    
}
