using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : Singleton<LetterManager>
{
    
    public event Action<LetterCarrier> OnLetterClicked;
    [SerializeField] LetterCarrier letterCarrierPrefab;
    
    public LetterCarrier SpawnLetterCarrier(Slot slot)
    {
        var letterCarrier = Instantiate(letterCarrierPrefab);
        letterCarrier.GetCarried(slot.WorldPosition);
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
    
    public void LetterGetsCarried(Slot carryingSlot, LetterCarrier letterCarrier)
    {
        letterCarrier.GetCarried(carryingSlot.WorldPosition);
    }
}