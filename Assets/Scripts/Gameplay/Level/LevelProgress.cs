using System.Collections.Generic;

class LevelProgress
{
    private int wordCount;
    private List<LevelWordStatus> levelWordsStatus;
    
    public LevelProgress(int wordCount)
    {
        this.wordCount = wordCount;
        levelWordsStatus = new List<LevelWordStatus>(new LevelWordStatus[wordCount]);
    }

    public int GetNonCompeteWordIndex()
    {
     return levelWordsStatus.IndexOf(LevelWordStatus.WordNotCompleted);
    } 
        
    
    public void SetLevelWordStatus(int index, LevelWordStatus levelWordStatus)
    {
        levelWordsStatus[index] = levelWordStatus;
    }
}