using UnityEngine;

public class Slot
{
    private Vector2 cords;
    private bool isOccupied;
    public SlotLocation SlotLocation { get; set; }
    
    private ICarryable carryable;
    public ICarryable Carryable
    {
        get => carryable;
        set => carryable = value;
    }

    public Vector2 Cords => cords;
    public bool IsOccupied=> isOccupied;

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
        carryable = null;
    }

    public void CarryItem(ICarryable carryable)
    {
        if (isOccupied)
            Debug.Log("You are trying to carry an item to an occupied slot!"); 

        this.carryable = carryable;
        isOccupied = true;
    }
}