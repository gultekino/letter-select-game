using System;
using System.Collections.Generic;
using UnityEngine;

#region EventData
public struct LetterClickedEvent : IEvent
{
    public LetterCarrier letterCarrier;
}
#endregion
public class LetterManager : Singleton<LetterManager>
{
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
        EventBus<LetterClickedEvent>.Raise(new LetterClickedEvent(){letterCarrier = letterCarrier});
    }

    public void MoveLetterGridB(LetterCarrier letterCarrier,Slot carryingSlot)
    {
        GridsManager.Instance.EmptyASlot(letterCarrier.CarryingSlot);
        letterCarrier.GetCarried(carryingSlot);
        carryingSlot.CarryItem(letterCarrier);
    }
}