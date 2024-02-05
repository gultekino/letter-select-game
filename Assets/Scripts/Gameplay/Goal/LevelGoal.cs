public class LevelGoal
{
    public LetterStatus[] lettersStatus;
    public  string goalWord;
    public int wordIndexOnLevel;
    public LevelGoal(string GoalWord,int wordIndexOnLevel)
    {
        goalWord = GoalWord;
        lettersStatus = new LetterStatus[GoalWord.Length];
        this.wordIndexOnLevel = wordIndexOnLevel;
    }
}