using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    private List<LevelGoal> levelGoals = new List<LevelGoal>();
    private LevelGoal activeGoal;
    public event Action<int> GoalWordCompleted;
    public event Action<int,int,int> GoalWordChanged; //int goalWordIndex, int previousGoalWordIndex, int goalWordLength

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeGoalToNext();
        }
    }
    

    private void ChangeGoalToNext()
    {
        SetupNextGoal();
    }

    private void HandleLevelStarted()
    {
        SetupFirstGoal();
    }

    private void SetupNextGoal()
    {
        var goalIndex = LevelManager.Instance.TryGetNextGoalIndex();
        if (goalIndex == -1)
            return;

        LevelManager.Instance.SetGoalWordIndex(goalIndex);
        int previousGoalWordIndex = activeGoal.WordIndex;
        activeGoal = levelGoals[goalIndex];
        GoalWordChanged?.Invoke(goalIndex, previousGoalWordIndex, levelGoals[goalIndex].GoalWord.Length);
    }
    
    private void SetupFirstGoal()
    {
        LevelManager.Instance.SetGoalWordIndex(0);
        activeGoal = levelGoals[0];
        GoalWordChanged?.Invoke(0, activeGoal.WordIndex, activeGoal.GoalWord.Length);
    }
    
    private void CompleteGoal()
    {
        GoalWordCompleted?.Invoke(activeGoal.WordIndex);
        SetupNextGoal();
    }
    
    private void Start()
    {
        var goalWords = LevelManager.Instance.GetGoalWords();
        for (int i = 0; i < goalWords.Count; i++)
        {
            levelGoals.Add(new LevelGoal(goalWords[i], i));
        }
        LevelManager.Instance.LevelStarted += HandleLevelStarted;
    }
    private void OnDisable()
    {
        LevelManager.Instance.LevelStarted -= HandleLevelStarted;
    }
    
    public int TryGetIndexOfLetterInTheGoal(LetterCarrier letterCarrier)
    {
        var letter = letterCarrier.GetLetter();
        var positionToFill = TryGetPositionToFill(letter);
        if (positionToFill == -1)
            return -1;
        
        return positionToFill;
    }
    
    private int TryGetPositionToFill(char letter)
    {
        var targetPositions = activeGoal.GoalWord.AllIndexesOf(letter).Where(pos=> activeGoal.IsPositionEmpty(pos));
        if (!targetPositions.Any())
        {
            return -1;
        }
        return targetPositions.First();
    }

    public void LetterInGoalSelected(int letterIndexInTheGoal)
    {
        activeGoal.MarkLetterAsFilled(letterIndexInTheGoal);
        if (activeGoal.IsGoalCompleted())
            CompleteGoal();
    }
}