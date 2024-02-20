using System;
using System.Linq;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    private LevelGoal activeGoal;
    public event Action<int> OnGoalWordCompleted;

    private void Start()
    {
        LevelManager.Instance.OnLevelStarted += OnLevelStarted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeGoal(activeGoal.wordIndexOnLevel);
        }
    }
    private void ChangeGoal(int wordIndex)
    {
        var (goalWord, goalIndex) = LevelManager.Instance.GetNextGoalWord();
        if (goalIndex == -1) return;
        SetUpNextGoal(goalWord, goalIndex);
    }


    private void OnLevelStarted()
    {
        var (goalWord, goalIndex) = LevelManager.Instance.GetNextGoalWord();
        activeGoal = new LevelGoal(goalWord, goalIndex);
        GridsManager.Instance.PrepareGridForGoalWord(goalWord.Length);
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnLevelStarted -= OnLevelStarted;
    }

    private bool LetterSelected(LetterCarrier chosenLetter)
    {
        var letter = chosenLetter.GetLetterCarrying();
        var goalWordLetterPositions = activeGoal.goalWord.AllIndexesOf(letter);
        if (!goalWordLetterPositions.Any()) return false;

        var unfilledPositions = goalWordLetterPositions
            .Where(position => activeGoal.lettersStatus[position] == LetterStatus.Empty);

        if (!unfilledPositions.Any()) return false;

        var positionToFill = unfilledPositions.First();
        activeGoal.lettersStatus[positionToFill] = LetterStatus.Filled;
        GridsManager.Instance.LetterNeededByGoal(chosenLetter, positionToFill);

        if (activeGoal.lettersStatus.All(status => status == LetterStatus.Filled))
        {
            LetterManager.Instance.MoveLettersToTable(activeGoal.wordIndexOnLevel);
            OnGoalWordCompleted?.Invoke(activeGoal.wordIndexOnLevel);
            SetUpNextGoal();
        }

        return true;
    }

    public void SetUpNextGoal(string a="",int b=0)
    {
        var (nextGoalWord, nextGoalIndex) = LevelManager.Instance.GetNextGoalWord();
        if (nextGoalWord == null) return;

        activeGoal = new LevelGoal(nextGoalWord, nextGoalIndex);
        GridsManager.Instance.PrepareGridForGoalWord(nextGoalWord.Length);
    }

    public bool PartOfTheGoal(LetterCarrier letterCarrier)
    {
        return LetterSelected(letterCarrier);
    }
}