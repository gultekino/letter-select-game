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
        var slot = TableManager.Instance.GetGoalSlot();//.GetGoalTableLoc(activeGoalWordIndexOnLevel);
        foreach (var letter in lettersInGoalGrid)
        {
            GridsManager.Instance.EmptyASlot(letter.CarryingSlot);
            letter.GetCarried(slot);
            slot.CarryItem(letter);
        }
        lettersInGoalGrid.Clear();
    }

    public void LetterNeededByGoal(LetterCarrier letterCarrier)
    {
        lettersInGoalGrid.Add(letterCarrier);
    }
}