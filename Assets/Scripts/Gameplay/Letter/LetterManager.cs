using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : Singleton<LetterManager>
{
    public event Action<LetterCarrier> OnLetterClicked;

    [SerializeField] LetterCarrier letterCarrierPrefab;
    List<LetterCarrier> lettersInGoalGrid = new List<LetterCarrier>();
    public LetterCarrier SpawnLetterCarrier(Slot slot)
    {
        var letterCarrier = Instantiate(letterCarrierPrefab);
        letterCarrier.GetCarried(slot);
        return letterCarrier;
    }

    public void LetterClicked(LetterCarrier letterCarrier)
    {
        OnLetterClicked?.Invoke(letterCarrier);
    }

    public void MoveLetterGridB(LetterCarrier letterCarrier,Slot carryingSlot)
    {
        GridsManager.Instance.EmptyASlot(letterCarrier.CarryingSlot);
        letterCarrier.GetCarried(carryingSlot);
        carryingSlot.CarryItem(letterCarrier);
    }

    public void MoveLettersToTable(int activeGoalWordIndexOnLevel)
    {
        var slot = TableManager.Instance.GetGoalTableSlots(activeGoalWordIndexOnLevel);
        for (var index = 0; index < lettersInGoalGrid.Count; index++)
        {
            var letter = lettersInGoalGrid[index];
            GridsManager.Instance.EmptyASlot(letter.CarryingSlot);
            lettersInGoalGrid[index].GetCarried(slot[index]);
            slot[index].CarryItem(lettersInGoalGrid[index]);
        }

        lettersInGoalGrid.Clear();
    }

    public void LetterNeededByGoal(LetterCarrier letterCarrier)
    {
        lettersInGoalGrid.Add(letterCarrier);
    }
}