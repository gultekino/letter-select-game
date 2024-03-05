using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    public SlotLocation SlotLocation { get; set; }
    private ICarryable LetterCarrier { get; set; }
    public Vector3 WorldPosition { get; set; }
    
    private Vector2 cords;
    private Vector2 itemTargetScale;
    private bool isOccupied;
    public bool IsOccupied => isOccupied;
    public Vector2 ItemTargetScale => itemTargetScale;
    
    public Slot(Vector2 cords, bool isOccupied, Vector3 worldPosition, SlotLocation slotLocation, Vector2 itemTargetScale)
    {
        this.cords = cords;
        this.isOccupied = isOccupied;
        this.itemTargetScale = itemTargetScale;
        SlotLocation = slotLocation;
        WorldPosition = worldPosition;
    }
 

    public void EmptySlot()
    {
        isOccupied = false;
        LetterCarrier = null;
    }

    public void CarryItem(LetterCarrier letterCarrier)
    {
        if (isOccupied)
            Debug.Log("You are trying to carry an item to an occupied slot!");

        this.LetterCarrier = letterCarrier;
        isOccupied = true;
    }

    public LetterCarrier GetCarriedItem()
    {
        return (LetterCarrier)LetterCarrier;
    }

    public void SwapItemsWith(Slot anotherSlot)
    {
        (anotherSlot.LetterCarrier, this.LetterCarrier) = (this.LetterCarrier, anotherSlot.LetterCarrier);
        (anotherSlot.isOccupied, this.isOccupied) = (this.isOccupied, anotherSlot.isOccupied);
        
        if (anotherSlot.IsOccupied)
            anotherSlot.LetterCarrier.GetCarried(anotherSlot);
        if (this.IsOccupied)
            this.LetterCarrier.GetCarried(this);
    }
}