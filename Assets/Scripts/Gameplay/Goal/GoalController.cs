using System;
using System.Collections.Generic;
using System.Linq;
using Array2DEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalModel
{
    public string GoalWord { get; private set; }
    public int WordIndex { get; private set; }
    private List<bool> lettersFilled;

    public LevelGoalModel(string goalWord, int wordIndex)
    {
        GoalWord = goalWord;
        WordIndex = wordIndex;
        lettersFilled = Enumerable.Repeat(false, goalWord.Length).ToList();
    }

    // Checks if a specific letter position has been filled
    public bool IsPositionFilled(int position)
    {
        if (position < 0 || position >= lettersFilled.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of the range of the goal word.");
        }
        return lettersFilled[position];
    }

    // Marks a letter at a specific position as filled
    private void FillLetterPosition(int position)
    {
        if (position < 0 || position >= lettersFilled.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of the range of the goal word.");
        }
        lettersFilled[position] = true;
    }

    // Determines if the goal has been completed (all letters filled)
    public bool IsGoalCompleted()
    {
        return lettersFilled.All(filled => filled);
    }

    // Finds all indexes of a given letter in the goal word that are not filled yet
    public IEnumerable<int> GetUnfilledLetterPositions(char letter)
    {
        for (int i = 0; i < GoalWord.Length; i++)
        {
            if (GoalWord[i] == letter && !lettersFilled[i])
            {
                yield return i;
            }
        }
    }

    public int TryGetPositionToFill(char letter)
    {
        var targetPositions = GetUnfilledLetterPositions(letter);
        if (!targetPositions.Any())
        {
            return -1;
        }
        return targetPositions.First();
    }

    public void MarkLetterAsFilled(int positionToFill)
    {
        FillLetterPosition(positionToFill);
    }

    public void ChangeActiveGoal(int index)
    {
        
    }
}


public class GoalView : MonoBehaviour
{
    // Reference to UI elements in the Unity Inspector
    [SerializeField] private Text goalWordText;
    [SerializeField] private Text progressText;

    // Method to update the displayed goal word
    public void UpdateGoalDisplay(string goalWord)
    {
        if (goalWordText != null)
        {
            goalWordText.text = "Goal: " + goalWord;
        }
    }

    // Method to update the progress towards completing the current goal
    public void UpdateProgressDisplay(int filledLetters, int totalLetters)
    {
        if (progressText != null)
        {
            progressText.text = $"Progress: {filledLetters}/{totalLetters}";
        }
    }

    // Optionally, if you have animations or special effects when a goal is completed
    public void ShowGoalCompletionEffect()
    {
        // Trigger animations or effects here
        Debug.Log("Goal Completed!");
    }
}

public class GoalController : MonoBehaviour
{
    // References to the model and view components
    private List<LevelGoalModel> levelGoals = new List<LevelGoalModel>();
    private LevelGoalModel activeGoal;
    [SerializeField] private GoalView goalView; // Reference to the view component

    void Start()
    {
        InitializeGoals();
        SetupFirstGoal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeGoalToNext();
        }
    }

    private void InitializeGoals()
    {
        // Assuming LevelManager is part of the model that provides goal words
        var goalWords = LevelManager.Instance.GetGoalWords();
        for (int i = 0; i < goalWords.Count; i++)
        {
            levelGoals.Add(new LevelGoalModel(goalWords[i], i));
        }
    }

    private void ChangeGoalToNext()
    {
        var goalIndex = LevelManager.Instance.TryGetNextGoalIndex(); // Model logic
        if (goalIndex == -1 || goalIndex >= levelGoals.Count)
            return;

        SetActiveGoal(goalIndex);
    }

    private void SetActiveGoal(int goalIndex)
    {
        activeGoal = levelGoals[goalIndex];
        LevelManager.Instance.SetGoalWordIndex(goalIndex); // Model logic
        goalView.UpdateGoalDisplay(activeGoal.GoalWord); // View update
        
        // Optionally, update progress display in view
        //goalView.UpdateProgressDisplay(activeGoal.GetFilledLettersCount(), activeGoal.GoalWord.Length);
    }

    private void SetupFirstGoal()
    {
        if (levelGoals.Count > 0)
        {
            SetActiveGoal(0);
        }
    }

    public void LetterInGoalSelected(char letter)
    {
        var positionToFill = activeGoal.TryGetPositionToFill(letter);
        if (positionToFill != -1)
        {
            activeGoal.MarkLetterAsFilled(positionToFill);
            //goalView.UpdateProgressDisplay(activeGoal.GetFilledLettersCount(), activeGoal.GoalWord.Length); // View update

            if (activeGoal.IsGoalCompleted())
            {
                goalView.ShowGoalCompletionEffect(); // View update for goal completion
                ChangeGoalToNext();
            }
        }
    }
}