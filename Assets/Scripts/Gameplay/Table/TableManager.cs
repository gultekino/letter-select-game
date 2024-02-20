using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TablePiece
{
    public int wordIndex;
    public int lineIndex;
    public Vector2 startLocation;
    public List<Slot> tableSlots;

    public TablePiece(int wordIndex, int lineIndex, Vector2 startLocation, List<Slot> tableSlots)
    {
        this.wordIndex = wordIndex;
        this.lineIndex = lineIndex;
        this.startLocation = startLocation;
        this.tableSlots = tableSlots;
    }
}

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private GameObject tablePiecePrefab;
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] private Transform tableParent;
    List<Transform> tableLocs = new List<Transform>();
    List<TablePiece> tablePieces = new List<TablePiece>();
    private int howManyLettersFit = 4;
    private int paddingBetweenTablePieces = 1;
    private Vector2 tableOffset = new Vector3(0, 10);
    
    private void Start()
    {
        InitializeTable();
    }

    private void InitializeTable()
    {
        var goalWords = levelDataSO.levelData.GoalWords;
        int letterLengthInTable = 0;
        int newLine = 0;
        for (int i = 0; i < goalWords.Count; i++)
        {
            var goalWord = goalWords[i];
            letterLengthInTable += goalWord.Length;
            if (letterLengthInTable > howManyLettersFit)
            {
                newLine++;
                letterLengthInTable = 0;
            }

            Vector2 tl = new Vector2(0, i) + tableOffset;
            GridConfiguration gridConfiguration = new GridConfiguration(goalWord.Length, 1, tl);
            GridSpawner spawner = new GridSpawner();
            var Slots = spawner.SpawnGrid(gridConfiguration, tablePiecePrefab, tableParent, SlotLocation.Table);
            var tp = new TablePiece(i, newLine, tl, Slots);
            tablePieces.Add(tp);
        }
    }

    public Transform GetGoalTableLoc(int activeGoalWordIndexOnLevel)
    {
        return tableLocs[activeGoalWordIndexOnLevel];
    }
    
    public List<Slot> GetGoalTableSlots(int activeGoalWordIndexOnLevel)
    {
        return tablePieces[activeGoalWordIndexOnLevel].tableSlots;
        //return tableLocs[activeGoalWordIndexOnLevel];
    }
}