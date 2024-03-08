using System;
using System.Collections.Generic;
using System.Linq;
using Array2DEditor;
using UnityEngine;

public struct GoalCompleteEvent : IEvent
{
    public int goalWordIndex;
}

public struct GoalChangedEvent : IEvent
{
    public int goalWordIndex;
    public int previousGoalWordIndex;
    public int goalWordLength;
}

public class GoalManager : Singleton<GoalManager>
{
    private List<LevelGoal> levelGoals = new List<LevelGoal>();
    private LevelGoal activeGoal;

    #region Unity Methods
    EventBinding<LevelEvent> levelEventBinding;
    private void Start()
    {
        var goalWords = LevelManager.Instance.GetGoalWords();
        for (int i = 0; i < goalWords.Count; i++)
        {
            levelGoals.Add(new LevelGoal(goalWords[i], i));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeGoalToNext();
        }
    }

    private void OnEnable()
    {
        levelEventBinding = new EventBinding<LevelEvent>(HandleLevelStarted);
        EventBus<LevelEvent>.Register(levelEventBinding);
    }

    private void OnDisable()
    {
        EventBus<LevelEvent>.Deregister(levelEventBinding);
    }
    #endregion

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
        EventBus<GoalChangedEvent>.Raise(new GoalChangedEvent(){goalWordIndex = goalIndex, previousGoalWordIndex = previousGoalWordIndex, goalWordLength = levelGoals[goalIndex].GoalWord.Length});
    }
    
    private void SetupFirstGoal()
    {
        LevelManager.Instance.SetGoalWordIndex(0);
        activeGoal = levelGoals[0];
        EventBus<GoalChangedEvent>.Raise(new GoalChangedEvent(){goalWordIndex = 0, previousGoalWordIndex = activeGoal.WordIndex, goalWordLength = activeGoal.GoalWord.Length});
    }
    
    private void CompleteGoal()
    {
        EventBus<GoalCompleteEvent>.Raise(new GoalCompleteEvent(){goalWordIndex = activeGoal.WordIndex});
        SetupNextGoal();
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

    public Array2DString GetGridMap()
    {
        return LevelManager.Instance.GetGridMap();
    }
}