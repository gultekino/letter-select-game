using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : Singleton<LetterManager>
{
    public event Action<LetterCarrier> OnLetterClicked;
    public event Action<LetterCarrier,Action<LetterCarrier,int>> OnLetterSelected;

    [SerializeField] LetterCarrier letterCarrierPrefab;
    
    public LetterCarrier SpawnLetterCarrier(Slot slot)
    {
        var letterCarrier = Instantiate(letterCarrierPrefab);
        letterCarrier.GetCarried(slot);
        return letterCarrier;
    }
    
    public bool SpawnLetterCarriers(List<Slot> slots)
    {
        foreach (var slot in slots)
            SpawnLetterCarrier(slot);
        
        return true;
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
        OnLetterSelected?.Invoke(letterCarrier,LetterNeededByGoal);
    }

    private void LetterNeededByGoal(LetterCarrier letterCarrier, int indexOfLetter)
    {
        
    }

    public void LetterGetsCarried(Slot carryingSlot, LetterCarrier letterCarrier)
    {
        letterCarrier.GetCarried(carryingSlot);
    }
}