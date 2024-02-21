using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private GameObject tablePiecePrefab;
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] private Transform tableParent;

    List<TablePiece> tablePieces = new List<TablePiece>();
    private int howManyLettersFit = 4;
    private int paddingBetweenTablePieces = 1;
    private Vector2 tableOffset = new Vector2(0, 10);
    
    private void Start()
    {
        InitializeTable();
    }

    private void InitializeTable()
    {
        var goalWords = levelDataSO.levelData.GoalWords;
        int letterCount = 0, newLineCount = 0;

        for (var i = 0; i < goalWords.Count; i++)
        {
            var goalWord = goalWords[i];
            AdjustForNewLine(ref letterCount, goalWord.Length, ref newLineCount);

            Vector2 startPosition = CalculateStartPosition(newLineCount);
            var gridConfiguration = new GridConfiguration(goalWord.Length, 1, startPosition);
            var slots = CreateGridSlots(gridConfiguration);

            tablePieces.Add(new TablePiece(slots, newLineCount, startPosition));
        }
    }
    private void AdjustForNewLine(ref int currentLetterCount, int wordLength, ref int lineCount)
    {
        currentLetterCount += wordLength;
        if (currentLetterCount > howManyLettersFit)
        {
            lineCount++;
            currentLetterCount = wordLength;
        }
    }

    private Vector2 CalculateStartPosition(int lineCount)
    {
        return new Vector2(0, lineCount) + tableOffset;
    }

    private List<Slot> CreateGridSlots(GridConfiguration config)
    {
        GridSpawner spawner = new GridSpawner();
        return spawner.SpawnGrid(config, tablePiecePrefab, tableParent, SlotLocation.Table);
    }

    public List<Slot> GetTableSlotsForGoal(int goalIndex)
    {
        if (goalIndex >= 0 && goalIndex < tablePieces.Count)
        {
            return tablePieces[goalIndex].Slots;
        }
        return new List<Slot>();
    }
}