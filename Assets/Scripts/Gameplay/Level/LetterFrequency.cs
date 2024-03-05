//Later write a editor code that calculates all the frequencies on build time
[System.Serializable]
public class LetterFrequency
{
    public char Letter;
    public int Frequency;

    public LetterFrequency(char letter, int frequency)
    {
        Letter = letter;
        Frequency = frequency;
    }
}