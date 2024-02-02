public class LevelGoal
{
    public LetterStatus[] lettersStatus;
    public  string goalWord;

    public LevelGoal(string GoalWord)
    {
        goalWord = GoalWord;
        lettersStatus = new LetterStatus[GoalWord.Length];
    }
}