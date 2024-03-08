using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableGridHandler : GridHandler
{
    [SerializeField] private List<Vector2> twoLineCenterPositions;
    [SerializeField] private List<Vector2> threeLineCenterPositions;
    [SerializeField] private int maxLettersInLine;
    private List<List<Slot>> tableSlots = new List<List<Slot>>();
    
    public override void InitializeGrid(int row = -1)
    {
        var goalWords = LevelManager.Instance.GetGoalWords();
        List<int> lettersDistribution = CalculateWordsDistribution(goalWords);
        CreateTableGrids(goalWords, lettersDistribution);
    }

    private void CreateTableGrids(List<string> goalWords, List<int> lettersDistribution)
    {
        int wordCount = 0;
        for (int i = 0; i < lettersDistribution.Count; i++)
        {
            CreateTableLine(goalWords, wordCount, wordCount+lettersDistribution[i], i);
            wordCount += lettersDistribution[i];
        }
    }

    private void CreateTableLine(List<string> goalWords, int startIndex, int endIndex, int lineIndex)
    {
        int howManyWordsInLine = endIndex - startIndex;
        int howManyLetterInLine = 0;
        for (int i = startIndex; i < endIndex; i++)
        {
            howManyLetterInLine += goalWords[i].Length;
        }
        Vector2 startPosition = CalculateStartPosition(howManyWordsInLine,howManyLetterInLine, lineIndex);
        Vector2 currentPosition = startPosition;
        GridSpawner spawner = new GridSpawner();
        for (int i = startIndex; i < endIndex; i++)
        {
            var goalWord = goalWords[i];
            var configuration = new GridConfiguration(goalWord.Length, 1, currentPosition, gridConfiguration.TargetScale, gridConfiguration.OffsetBetweenSlots);
            var slots = spawner.SpawnGrid(configuration, slotPrefab, slotsParent, slotLocation, currentPosition);
            Slots.AddRange(slots);
            tableSlots.Add(slots);
            currentPosition.x += goalWord.Length * gridConfiguration.OffsetBetweenSlots.x + gridConfiguration.OffsetBetweenSlots.x;
        }
    }

    private Vector2 CalculateStartPosition(int howManyWordsInLine, int howManyLettersInLine, int lineIndex)
    {
        Vector2 startPosition = new Vector2(0, 0);
        if (howManyWordsInLine == 1)
        {
            startPosition = twoLineCenterPositions[lineIndex];
        }
        else if (howManyWordsInLine == 2)
        {
            startPosition = twoLineCenterPositions[lineIndex];
        }
        else if (howManyWordsInLine == 3)
        {
            startPosition = threeLineCenterPositions[lineIndex];
        }
        // Calculate total width: sum of word lengths + offsets between them
        float totalWidth = (howManyLettersInLine-1) * gridConfiguration.OffsetBetweenSlots.x + (howManyWordsInLine - 1) * gridConfiguration.OffsetBetweenSlots.x;
        // Centering: startPosition.x should start from half of totalWidth to the left
        startPosition.x -= totalWidth / 2;
        return startPosition;
    }

    private List<int> CalculateWordsDistribution(List<string> goalWords)
    {
        List<int> wordsDistribution = new List<int>();
        int letterCount = 0, wordCountForLetter = 0;

        foreach (var goalWord in goalWords)
        {
            if (letterCount + goalWord.Length > maxLettersInLine)
            {
                if (wordCountForLetter > 0)
                {
                    wordsDistribution.Add(wordCountForLetter);
                    wordCountForLetter = 0; 
                    letterCount = 0; 
                }
            }
            letterCount += goalWord.Length; 
            wordCountForLetter++; 
        }
        
        if (wordCountForLetter > 0)
        {
            wordsDistribution.Add(wordCountForLetter);
        }

        return wordsDistribution;
    }

    public void FillWordInTable(List<Slot> goalSlots, int goalWordIndex)
    {
        var tableSlot = tableSlots[goalWordIndex];
        for (int i = 0; i < goalSlots.Count; i++)
        {
            var slot = goalSlots[i];
            var letterCarrier = slot.GetCarriedItem();
            if (letterCarrier)
            {
                PlaceLetterInTableGrid(letterCarrier, i, goalWordIndex);
            }
        }
    }

    private void PlaceLetterInTableGrid(LetterCarrier letterCarrier, int i, int goalWordIndex)
    {
        letterCarrier.CarryingSlot.EmptySlot();
        var slot = tableSlots[goalWordIndex][i];
        slot.CarryItem(letterCarrier);
        letterCarrier.GetCarried(slot);
    }

    public List<Slot> GetTableSlotsForGoal(int wordIndex)
    {
        return tableSlots[wordIndex];
    }
}