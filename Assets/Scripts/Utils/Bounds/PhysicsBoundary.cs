using UnityEngine;

/// <summary>
/// Physics-based boundary constraint for Rigidbody2D objects
/// Applies forces to keep objects within camera bounds
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsBoundary : MonoBehaviour
{
    [Header("Physics Boundary Settings")]
    public float boundaryForce = 50f;
    public float dampingForce = 10f;
    public bool useColliderBounds = true;
    
    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private Vector2 colliderOffset;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Get collider for size calculations
        if (useColliderBounds)
        {
            objectCollider = GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                Bounds bounds = objectCollider.bounds;
                colliderOffset = new Vector2(bounds.size.x / 2f, bounds.size.y / 2f);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (Util.Instance != null && rb != null)
        {
            ApplyBoundaryForces();
        }
    }
    
    /// <summary>
    /// Applies forces to keep the object within camera bounds
    /// </summary>
    void ApplyBoundaryForces()
    {
        Vector3 position = transform.position;
        Bounds cameraBounds = Util.Instance.GetCameraBounds();
        Vector2 force = Vector2.zero;
        
        // Calculate effective bounds considering collider size
        float minX = cameraBounds.min.x + (useColliderBounds ? colliderOffset.x : 0);
        float maxX = cameraBounds.max.x - (useColliderBounds ? colliderOffset.x : 0);
        float minY = cameraBounds.min.y + (useColliderBounds ? colliderOffset.y : 0);
        float maxY = cameraBounds.max.y - (useColliderBounds ? colliderOffset.y : 0);
        
        // Check X boundaries
        if (position.x < minX)
        {
            float penetration = minX - position.x;
            force.x = boundaryForce * penetration;
            
            // Apply damping if moving towards the boundary
            if (rb.linearVelocity.x < 0)
                force.x += -rb.linearVelocity.x * dampingForce;
        }
        else if (position.x > maxX)
        {
            float penetration = position.x - maxX;
            force.x = -boundaryForce * penetration;
            
            // Apply damping if moving towards the boundary
            if (rb.linearVelocity.x > 0)
                force.x += -rb.linearVelocity.x * dampingForce;
        }
        
        // Check Y boundaries
        if (position.y < minY)
        {
            float penetration = minY - position.y;
            force.y = boundaryForce * penetration;
            
            // Apply damping if moving towards the boundary
            if (rb.linearVelocity.y < 0)
                force.y += -rb.linearVelocity.y * dampingForce;
        }
        else if (position.y > maxY)
        {
            float penetration = position.y - maxY;
            force.y = -boundaryForce * penetration;
            
            // Apply damping if moving towards the boundary
            if (rb.linearVelocity.y > 0)
                force.y += -rb.linearVelocity.y * dampingForce;
        }
        
        // Apply the calculated force
        if (force.magnitude > 0)
        {
            rb.AddForce(force);
        }
    }
}
