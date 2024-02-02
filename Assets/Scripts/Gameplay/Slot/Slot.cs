using UnityEngine;

public enum SlotLocation
{
    Default=-1,
    GridA,
    GridB,
    GridC
}
public class Slot
{
    private Vector2 cords;
    private bool isOccupied;
    private ICarryable carryable;
    public SlotLocation SlotLocation { get; set; }
    public ICarryable Carryable
    {
        get => carryable;
        set => carryable = value;
    }

    public Vector2 Cords => cords;
    public bool IsOccupied
    {
        get => isOccupied;
        set => isOccupied = value;
    }

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
}