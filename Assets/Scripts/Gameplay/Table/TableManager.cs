using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private GameObject tablePiecePrefab;
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] private Transform tableParent;
    List<Transform> tableLocs = new List<Transform>();
    
    private int howManyLettersFit = 12;
    private int paddingBetweenTablePieces = 1;
    
    private void Start()
    {
        InitializeTable();
    }

    private void InitializeTable()
    {
        var goalWords = levelDataSO.levelData.GoalWords;
        int tablePieceCount = 0;
        for (int i = 0; i <goalWords.Count; i++)
        {
            if (tablePieceCount == 0)
            {
                var tablePiece = Instantiate(tablePiecePrefab, tableParent);
                tablePiece.transform.position = new Vector2(i * paddingBetweenTablePieces, 0);
                tableLocs.Add(tablePiece.transform);
                tablePieceCount+= goalWords[i].Length;
            }
            
            if(tablePieceCount > howManyLettersFit)
                tablePieceCount = 0;
        }
    }

    public Transform GetGoalTableLoc(int activeGoalWordIndexOnLevel)
    {
        return tableLocs[activeGoalWordIndexOnLevel];
    }
}