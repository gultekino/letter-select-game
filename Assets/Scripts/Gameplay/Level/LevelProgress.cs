using System.Collections.Generic;
using System.Linq;

class LevelProgress
{
    private int wordCount;
    private List<LevelWordStatus> levelWordsStatus;
    public int CurrentWordIndex { get; set; }

    public LevelProgress(int wordCount)
    {
        this.wordCount = wordCount;
        levelWordsStatus = Enumerable.Repeat(LevelWordStatus.WordNotCompleted, wordCount).ToList();
    }

    public int GetNextIncompleteWordIndex()
    {
        int nextWordIndex = levelWordsStatus.IndexOf(LevelWordStatus.WordNotCompleted, CurrentWordIndex + 1);
        if (nextWordIndex == -1)
        {
            nextWordIndex = GetFirstIncompleteWordIndex();
        }

        return nextWordIndex;
    }
    
    public int GetFirstIncompleteWordIndex()
    {
        return levelWordsStatus.IndexOf(LevelWordStatus.WordNotCompleted);
    }
    
    public bool AllWordsCompleted()
    {
        return levelWordsStatus.All(status => status == LevelWordStatus.WordCompleted);
    }
    
    public void SetLevelWordStatus(int index, LevelWordStatus status)
    {
        levelWordsStatus[index] = status;
    }
}