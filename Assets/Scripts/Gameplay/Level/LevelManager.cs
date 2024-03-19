using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array2DEditor;
using UnityEngine;

#region EventData
public struct LevelEvent : IEvent
{
}

#endregion

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelDataSO levelDataSO;
    LevelProgress levelProgress;
    private LevelData levelData => levelDataSO.levelData;
    
    EventBinding<GoalCompleteEvent> goalCompleteEventBinding;
    EventBinding<GoalChangedEvent> goalChangedEventBinding;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        levelProgress = new LevelProgress(levelData.GoalWords.Count);
        yield return null; // Ensures other managers are ready and subscribed
        EventBus<LevelEvent>.Raise(new LevelEvent());
    }

    private void OnEnable()
    {
        goalCompleteEventBinding = new EventBinding<GoalCompleteEvent>(HandleWordCompletion);
        EventBus<GoalCompleteEvent>.Register(goalCompleteEventBinding);
        
        goalChangedEventBinding = new EventBinding<GoalChangedEvent>(HandleGoalWordChanged);
        EventBus<GoalChangedEvent>.Register(goalChangedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<GoalCompleteEvent>.Deregister(goalCompleteEventBinding);
        EventBus<GoalChangedEvent>.Deregister(goalChangedEventBinding);
    }

    private void HandleGoalWordChanged(GoalChangedEvent goalChangedEvent)
    {
        levelProgress.SetLevelWordStatus(goalChangedEvent.goalWordIndex,LevelWordStatus.WordNotCompleted);
        levelProgress.CurrentWordIndex = goalChangedEvent.goalWordIndex;
    }

    private void HandleWordCompletion(GoalCompleteEvent wordIndexOnLevel)
    {
        UpdateWordCompletionStatus(wordIndexOnLevel.goalWordIndex,LevelWordStatus.WordCompleted);
        CheckForLevelCompletion();
    }
    
    private void UpdateWordCompletionStatus(int wordIndexOnLevel, LevelWordStatus levelWordStatus)
    {
        levelProgress.SetLevelWordStatus(wordIndexOnLevel, levelWordStatus);
    }
    
    private void CheckForLevelCompletion()
    {
        if (levelProgress.AllWordsCompleted())
        {
            Debug.Log("Level completed!");
        }
    }

    public int TryGetNextGoalIndex()
    {
        int wordIndex = levelProgress.GetNextIncompleteWordIndex();
        Debug.Log("Next word index: " + wordIndex);
        if (wordIndex == levelProgress.CurrentWordIndex)
        {
            Debug.Log("NO OTHER WORD LEFT");
            return -1;
        }

        if (wordIndex == -1)
        {
            Debug.Log("All words completed. Level ends.");
            return -1;
        }

        return wordIndex;
    }

    public List<string> GetGoalWords()
    {
        return levelData.GoalWords;
    }

    public void SetGoalWordIndex(int goalIndex)
    {
        levelProgress.CurrentWordIndex = goalIndex;
    }
    
    public Array2DString GetGridMap()
    {
        return levelData.GridMap;
    }
}