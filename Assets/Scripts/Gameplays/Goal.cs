using System;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public uint score = 0; // Score for this goal

    public GoalEvent onGoalScored;

    [Serializable]
    public sealed class GoalEvent : UnityEvent<uint> { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ball ball))
        {
            // Handle goal logic here, e.g., increase score, reset ball position, etc.
            Debug.Log("Goal scored!");
            score++;
            onGoalScored?.Invoke(score);
            // Reset the ball or perform other actions as needed
        }
    }
}
