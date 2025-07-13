using UnityEngine;

/// <summary>
/// Example player controller that demonstrates boundary usage
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool usePhysicsMovement = false;
    
    [Header("Boundary Settings")]
    public bool useBoundaryConstraint = true;
    
    private Rigidbody2D rb;
    private BoundaryConstraint boundaryConstraint;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boundaryConstraint = GetComponent<BoundaryConstraint>();
    }
    
    void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        
        if (usePhysicsMovement && rb != null)
        {
            // Physics-based movement
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            // Direct transform movement
            Vector3 newPosition = transform.position + (Vector3)movement * moveSpeed * Time.deltaTime;
            
            // Apply boundary constraint if enabled
            if (useBoundaryConstraint)
            {
                newPosition = Util.Instance.ClampToCameraBounds(newPosition);
            }
            
            transform.position = newPosition;
        }
    }
    
    // Alternative movement method for touch/mobile input
    public void MoveToPosition(Vector3 targetPosition)
    {
        if (useBoundaryConstraint)
        {
            targetPosition = Util.Instance.ClampToCameraBounds(targetPosition);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
