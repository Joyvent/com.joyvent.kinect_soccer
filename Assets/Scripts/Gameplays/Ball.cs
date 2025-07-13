using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    public void BeenKicked(Vector2 force)
    {
        if (rb != null)
        {
            rb.AddForce(force);
        }
    }

    private void FixedUpdate()
    {
        // Apply damp to simulate ball slowing down over time
        float dampFactor = 0.98f; // Adjust this value to control how quickly the ball slows down
        rb.linearVelocity *= dampFactor;

        // Simulate z-gravity by gradually reducing speed
        if (rb.linearVelocity.magnitude > 0.1f) // Only slow down if the ball is moving
        {
            // Additional slow down to simulate friction and z-gravity effect
            float zGravityEffect = 0.995f; // Adjust this for more/less gravity effect
            rb.linearVelocity *= zGravityEffect;
        }
        else if (rb.linearVelocity.magnitude < 0.1f)
        {
            // Stop the ball completely when it's moving very slowly
            rb.linearVelocity = Vector2.zero;
        }
    }
}
