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
    public event Action OnLevelStarted;
    private int currentWordIndex = 0;
    
    IEnumerator Start()
    {
        currentLevelProgress = new LevelProgress(levelData.GoalWords.Count);
        yield return null; // wait for other managers to subscribe to the event
        OnLevelStarted?.Invoke();
        GoalManager.Instance.OnGoalWordCompleted += WordCompleted;
    }
    
    private void LetterSelected(LetterCarrier arg1) //Game end condition
    {
        if (GridsManager.Instance.GetGridBEmptySlotCount()==0)
            Debug.Log("No empty slots in grid B");
    }

    public (string,int) GetNextGoalWord()
    {
        currentLevelProgress.SetLevelWordStatus(currentWordIndex,LevelWordStatus.WordNotCompleted);
        
        var wordIndex = currentLevelProgress.GetNonCompeteWordIndex();
        if (wordIndex == -1)
        {
            Debug.Log("No more words to compete. Game end.");
            return (null,-1);
        }
        
        currentLevelProgress.SetLevelWordStatus(wordIndex, LevelWordStatus.WordIsGoal);
        return (levelData.GoalWords[wordIndex],wordIndex);
    }

    public void WordCompleted(int wordIndexOnLevel)
    {
        currentLevelProgress.SetLevelWordStatus(wordIndexOnLevel, LevelWordStatus.WordCompleted);
    }
}