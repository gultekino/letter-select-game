using System.Collections.Generic;
using Array2DEditor;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;
    
    [SerializeField] private List<string> goalWords;
    public List<string> GoalWords => goalWords;
    
    [SerializeField] private Array2DString gridMap;
    public Array2DString GridMap => gridMap;
}