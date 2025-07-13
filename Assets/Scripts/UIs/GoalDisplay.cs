using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GoalDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _goalText;

    private void Awake()
    {
        _goalText = GetComponent<TMP_Text>();
        if (_goalText == null)
        {
            Debug.LogError("GoalDisplay requires a TMP_Text component.");
        }
    }

    public void SetGoal(uint goals)
    {
        SetGoalPrivate(goals);
    }

    public void SetGoal(int goals)
    {
        SetGoalPrivate(goals);
    }

    public void ClearGoal()
    {
        SetGoalPrivate(0);
    }

    private void SetGoalPrivate<T>(T goals)
    {
        if (_goalText != null)
        {
            _goalText.text = $"Goals: {goals}";
        }
        else
        {
            Debug.LogError("TMP_Text component is not assigned in GoalDisplay.");
        }
    }
}
