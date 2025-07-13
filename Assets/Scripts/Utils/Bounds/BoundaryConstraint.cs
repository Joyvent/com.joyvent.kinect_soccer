using UnityEngine;

/// <summary>
/// Attach this script to any object that should be constrained within camera bounds
/// </summary>
public class BoundaryConstraint : MonoBehaviour
{
    [Header("Boundary Settings")]
    public bool constrainToCamera = true;
    public bool useColliderBounds = true; // Consider object's collider size
    
    private Collider2D objectCollider;
    private Vector2 colliderOffset;
    
    void Start()
    {
        // Get collider for size calculations
        if (useColliderBounds)
        {
            objectCollider = GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                // Calculate offset based on collider bounds
                Bounds bounds = objectCollider.bounds;
                colliderOffset = new Vector2(bounds.size.x / 2f, bounds.size.y / 2f);
            }
        }
    }
    
    void Update()
    {
        if (constrainToCamera)
        {
            ApplyBoundaryConstraint();
        }
    }
    
    /// <summary>
    /// Applies boundary constraint considering object's collider size
    /// </summary>
    void ApplyBoundaryConstraint()
    {
        Vector3 currentPosition = transform.position;
        Vector3 constrainedPosition = currentPosition;
        
        if (useColliderBounds && objectCollider != null)
        {
            // Get camera bounds
            Bounds cameraBounds = Util.Instance.GetCameraBounds();
            
            // Apply constraints considering collider size
            float minX = cameraBounds.min.x + colliderOffset.x;
            float maxX = cameraBounds.max.x - colliderOffset.x;
            float minY = cameraBounds.min.y + colliderOffset.y;
            float maxY = cameraBounds.max.y - colliderOffset.y;
            
            constrainedPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
            constrainedPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);
        }
        else
        {
            // Use simple position constraint
            constrainedPosition = Util.Instance.ClampToCameraBounds(currentPosition);
        }
        
        transform.position = constrainedPosition;
    }
    
    /// <summary>
    /// Manually apply boundary constraint (useful for physics objects)
    /// </summary>
    public void ForceApplyConstraint()
    {
        ApplyBoundaryConstraint();
    }
}
