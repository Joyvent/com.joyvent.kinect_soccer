using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [Header("Settings")]
    public float kickForce = 10f; // Default kick force
    [SerializeField]
    private GoalDisplay playerGoalDisplay;
    [SerializeField]
    private GoalDisplay opponentGoalDisplay;
    [SerializeField]
    private Goal playerGoal;
    [SerializeField]
    private Goal opponentGoal;

    void Awake()
    {
        playerGoal.onGoalScored.AddListener(opponentGoalDisplay.SetGoal);
        opponentGoal.onGoalScored.AddListener(playerGoalDisplay.SetGoal);
        playerGoalDisplay.ClearGoal();
        opponentGoalDisplay.ClearGoal();
    }

    void Update()
    {
        // Handle mouse clicks for kicking the ball
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        if (mouseClicked)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.Log($"Mouse clicked at {mousePosition}, Ray hit: {hit.collider != null}");

            if (hit.collider != null && hit.collider.TryGetComponent(out Ball ball))
            {
                // Get the direction from mouse to ball
                KickBall(mousePosition, hit.collider.transform, ball);
            }
        }
    }

    private void KickBall(Vector3 kickPosition, Transform hit, Ball ball)
    {
        Vector2 ballPosition = hit.position;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(kickPosition);
        Vector2 direction = (ballPosition - mouseWorldPos).normalized;

        // Add force to kick the ball
        if (ball != null)
        {
            // Adjust the kick force as needed
            ball.BeenKicked(direction * kickForce);
        }
    }
}