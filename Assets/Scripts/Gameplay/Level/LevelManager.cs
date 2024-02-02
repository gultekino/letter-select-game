using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelDataSO levelDataSO;
    private LevelData levelData => levelDataSO.levelData;
    
    LevelProgress currentLevelProgress;
    public event Action OnLevelStart;
    
    IEnumerator Start()
    {
        currentLevelProgress = new LevelProgress(levelData.GoalWords.Count);
        yield return null; // wait for other managers to subscribe to the event
        OnLevelStart?.Invoke();
    }

    public string TryGetGoalWord()
    {
        var wordIndex = currentLevelProgress.GetNonCompeteWordIndex();
        if (wordIndex == -1)
            return null;
        
        currentLevelProgress.SetLevelWordStatus(wordIndex, LevelWordStatus.WordIsGoal);
        return levelData.GoalWords[wordIndex];
    }
}
public enum LevelWordStatus
{
    Default=-1,
    WordNotCompleted,
    WordIsGoal,
    WordCompleted
}
class LevelProgress
{
    private int wordCount;
    private List<LevelWordStatus> levelWordsStatus;
    
    public LevelProgress(int wordCount)
    {
        this.wordCount = wordCount;
        levelWordsStatus = new List<LevelWordStatus>(new LevelWordStatus[wordCount]);
    }

    public int GetNonCompeteWordIndex() => levelWordsStatus.IndexOf(LevelWordStatus.WordNotCompleted);
    
    public void SetLevelWordStatus(int index, LevelWordStatus levelWordStatus)
    {
        levelWordsStatus[index] = levelWordStatus;
    }
}