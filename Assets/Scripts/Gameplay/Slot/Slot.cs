using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    private Vector2 cords;
    private bool isOccupied;
    public SlotLocation SlotLocation { get; set; }

    private ICarryable letterCarrier;

    public ICarryable LetterCarrier
    {
        get => letterCarrier;
        set => letterCarrier = value;
    }

    public Vector2 Cords => cords;
    public bool IsOccupied => isOccupied;

    public Vector3 WorldPosition { get; set; }

    public Slot(Vector2 cords, bool isOccupied, Vector3 worldPosition, SlotLocation slotLocation)
    {
        this.cords = cords;
        this.isOccupied = isOccupied;
        WorldPosition = worldPosition;
        this.SlotLocation = slotLocation;
    }

    public void EmptySlot()
    {
        isOccupied = false;
        letterCarrier = null;
    }

    public void CarryItem(LetterCarrier letterCarrier)
    {
        if (isOccupied)
            Debug.Log("You are trying to carry an item to an occupied slot!");

        this.letterCarrier = letterCarrier;
        isOccupied = true;
    }

    public LetterCarrier GetCarriedItem()
    {
        return (LetterCarrier)letterCarrier;
    }

    public void ChangeLetterWithAnotherSlot(Slot anotherSlot)
    {
        (anotherSlot.letterCarrier, this.letterCarrier) = (this.letterCarrier, anotherSlot.letterCarrier);
        (anotherSlot.isOccupied, this.isOccupied) = (this.isOccupied, anotherSlot.isOccupied);
        
        if (anotherSlot.IsOccupied)
            anotherSlot.letterCarrier.GetCarried(anotherSlot);
        if (this.IsOccupied)
            this.letterCarrier.GetCarried(this);
    }
}