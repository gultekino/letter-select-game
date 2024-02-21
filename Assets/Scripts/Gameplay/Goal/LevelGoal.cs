using System.Linq;

public class LevelGoal
{
    public LetterStatus[] LettersStatus;
    public  string GoalWord;
    public int WordIndex;
    public LevelGoal(string goalWord,int wordIndex)
    {
        this.GoalWord = goalWord;
        LettersStatus = new LetterStatus[goalWord.Length];
        this.WordIndex = wordIndex;
    }
    
    public bool IsPositionEmpty(int position)
    {
        return LettersStatus[position] == LetterStatus.Empty;
    }

    public void MarkLetterAsFilled(int positionToFill)
    {
        LettersStatus[positionToFill] = LetterStatus.Filled;
    }

    public bool IsGoalCompleted()
    {
        return LettersStatus.All(status => status == LetterStatus.Filled);
    }
}