using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : Singleton<LetterManager>
{
    public event Action<LetterCarrier> OnLetterClicked;

    [SerializeField] LetterCarrier letterCarrierPrefab;
    public LetterCarrier SpawnLetterCarrier(Slot slot,char letter)
    {
        var letterCarrier = Instantiate(letterCarrierPrefab);
        letterCarrier.GetCarried(slot);
        letterCarrier.SetLetter(letter);
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
}