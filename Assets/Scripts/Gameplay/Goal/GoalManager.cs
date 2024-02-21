using System;
using System.Linq;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    private LevelGoal activeGoal;
    public event Action<int> GoalWordCompleted;
    public event Action<string, int> GoalWordChanged;

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
        
        SetupGoal(nextWord, nextIndex);
        GoalWordChanged?.Invoke(nextWord, nextIndex);
    }

    private void SetupGoal(string goalWord, int goalIndex)
    {
        activeGoal = new LevelGoal(goalWord, goalIndex);
        GridsManager.Instance.PrepareGridForGoalWord(activeGoal.GoalWord.Length);
    }

    private void HandleLevelStarted()
    {
        SetupFirstGoal();
    }

    private void SetupFirstGoal()
    {
        var (goalWord, goalIndex) = LevelManager.Instance.TryGetFirstGoalWord();
        SetupGoal(goalWord,goalIndex);
    }

    private void SetupNextGoal()
    {
        var (goalWord, goalIndex) = LevelManager.Instance.TryGetNextGoalWord();
        SetupGoal(goalWord,goalIndex);
    }
    
    private void CompleteGoal()
    {
        LetterManager.Instance.MoveLettersToTable(activeGoal.WordIndex);
        GoalWordCompleted?.Invoke(activeGoal.WordIndex);
        SetupNextGoal();
    }
    
    private void Start()
    {
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

        var positionToFill = targetPositions.FirstOrDefault(pos => activeGoal.IsPositionEmpty(pos));
        if (positionToFill == -1)
        {
            return -1;
        }

        return positionToFill;
    }
    public bool PartOfTheGoal(LetterCarrier letterCarrier)
    {
        return TrySelectLetter(letterCarrier);
    }
}