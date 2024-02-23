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
        var (nextWord, nextIndex) = LevelManager.Instance.TryGetNextGoalWord();
        if (nextIndex == -1) return;
        
        GoalWordChanged?.Invoke(nextIndex, activeGoal.WordIndex, activeGoal.GoalWord.Length);
    }

    private void HandleLevelStarted()
    {
        SetupNextGoal();
    }

    private void SetupNextGoal()
    {
        var (goalWord, goalIndex) = LevelManager.Instance.TryGetNextGoalWord();
        activeGoal = levelGoals[goalIndex];
        GoalWordChanged?.Invoke(goalIndex, activeGoal.WordIndex, activeGoal.GoalWord.Length);
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

    private bool TrySelectLetter(LetterCarrier letterCarrier)
    {
        var letter = letterCarrier.GetLetter();
        var positionToFill = TryGetPositionToFill(letter);
        if (positionToFill == -1)
            return false;

        activeGoal.MarkLetterAsFilled(positionToFill);
        GridsManager.Instance.PlaceLetterInGoal(letterCarrier, positionToFill);

        if (activeGoal.IsGoalCompleted())
            CompleteGoal();

        return true;
    }
    
    private int TryGetPositionToFill(char letter)
    {
        var targetPositions = activeGoal.GoalWord.AllIndexesOf(letter);
        if (!targetPositions.Any())
        {
            return -1;
        }

        var positionToFill = targetPositions.Where(pos => activeGoal.IsPositionEmpty(pos));
        if (!positionToFill.Any())
        {
            return -1;
        }

        return positionToFill.First();
    }
    public bool PartOfTheGoal(LetterCarrier letterCarrier)
    {
        return TrySelectLetter(letterCarrier);
    }
}