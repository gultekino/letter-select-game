using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelDataSO levelDataSO;
    private LevelData levelData => levelDataSO.levelData;
    
    LevelProgress levelProgress;
    public event Action LevelStarted;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        levelProgress = new LevelProgress(levelData.GoalWords.Count);
        GoalManager.Instance.GoalWordCompleted += HandleWordCompletion;
        GoalManager.Instance.GoalWordChanged += HandleGoalWordChanged;
        yield return null; // Ensures other managers are ready and subscribed
        LevelStarted?.Invoke();
    }

    private void HandleGoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
    {
        levelProgress.SetLevelWordStatus(goalWordIndex,LevelWordStatus.WordNotCompleted);
        levelProgress.CurrentWordIndex = goalWordIndex;
    }

    private void HandleWordCompletion(int wordIndexOnLevel)
    {
        UpdateWordCompletionStatus(wordIndexOnLevel,LevelWordStatus.WordCompleted);
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

    public List<LetterFrequency> GetLetterFrequencies()
    {
        return levelData.LetterFrequencies;
    }
}