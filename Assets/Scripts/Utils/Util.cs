using UnityEngine;
using UnityEngine.InputSystem;

public class Util : MonoBehaviour
{
    private static Util instance;

    public static Util Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetInstance();
            }
            return instance;
        }
    }

    private static Util GetInstance()
    {
        var controller = FindFirstObjectByType<Util>();
        if (controller == null)
        {
            controller = new GameObject("GameController").AddComponent<Util>();
        }
        DontDestroyOnLoad(controller.gameObject);
        return controller;
    }

    [Header("Camera Bounds Settings")]
    public Camera gameCamera;
    public bool enableBounds = true;
    public float boundaryOffset = 0.5f; // Extra space from camera edge

    private Vector2 minBounds;
    private Vector2 maxBounds;

    void Start()
    {
        // If no camera is assigned, use the main camera
        if (gameCamera == null)
            gameCamera = Camera.main;

        CalculateCameraBounds();
        Physics2D.gravity = new Vector2(0, 0); // Set default gravity
    }

    void Update()
    {
        if (enableBounds)
        {
            CalculateCameraBounds();
        }
    }
    
    /// <summary>
    /// Calculates the world space boundaries of the camera view
    /// </summary>
    void CalculateCameraBounds()
    {
        if (gameCamera == null) return;
        
        // Get camera bounds in world space
        Vector3 bottomLeft = gameCamera.ScreenToWorldPoint(new Vector3(0, 0, gameCamera.nearClipPlane));
        Vector3 topRight = gameCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, gameCamera.nearClipPlane));
        
        // Apply boundary offset
        minBounds = new Vector2(bottomLeft.x - boundaryOffset, bottomLeft.y - boundaryOffset);
        maxBounds = new Vector2(topRight.x + boundaryOffset, topRight.y + boundaryOffset);
    }
    
    /// <summary>
    /// Clamps a position to stay within camera bounds
    /// </summary>
    /// <param name="position">The position to clamp</param>
    /// <returns>The clamped position</returns>
    public Vector3 ClampToCameraBounds(Vector3 position)
    {
        if (!enableBounds) return position;
        
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        
        return position;
    }
    
    /// <summary>
    /// Clamps a 2D position to stay within camera bounds
    /// </summary>
    /// <param name="position">The position to clamp</param>
    /// <returns>The clamped position</returns>
    public Vector2 ClampToCameraBounds(Vector2 position)
    {
        if (!enableBounds) return position;
        
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        
        return position;
    }
    
    /// <summary>
    /// Checks if a position is within camera bounds
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>True if within bounds, false otherwise</returns>
    public bool IsWithinCameraBounds(Vector3 position)
    {
        return position.x >= minBounds.x && position.x <= maxBounds.x &&
               position.y >= minBounds.y && position.y <= maxBounds.y;
    }
    
    /// <summary>
    /// Gets the current camera bounds
    /// </summary>
    /// <returns>A Bounds object representing the camera area</returns>
    public Bounds GetCameraBounds()
    {
        Vector2 center = (minBounds + maxBounds) / 2f;
        Vector2 size = maxBounds - minBounds;
        return new Bounds(center, size);
    }
    
    /// <summary>
    /// Applies boundary constraints to a transform component
    /// Call this method for any object that should stay within bounds
    /// </summary>
    /// <param name="objectTransform">The transform to constrain</param>
    public void ApplyBoundaryConstraint(Transform objectTransform)
    {
        if (!enableBounds || objectTransform == null) return;
        
        objectTransform.position = ClampToCameraBounds(objectTransform.position);
    }
    
    // Optional: Draw bounds in the scene view for debugging
    void OnDrawGizmosSelected()
    {
        if (!enableBounds) return;
        
        Gizmos.color = Color.red;
        Vector2 center = (minBounds + maxBounds) / 2f;
        Vector2 size = maxBounds - minBounds;
        Gizmos.DrawWireCube(center, size);
    }
}
