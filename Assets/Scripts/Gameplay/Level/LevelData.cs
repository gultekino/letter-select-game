using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;
    
    [SerializeField] private List<string> goalWords;
    public List<string> GoalWords => goalWords;
    
    [SerializeField] private List<LetterFrequency> letterFrequencies;
    public List<LetterFrequency> LetterFrequencies => letterFrequencies;
}