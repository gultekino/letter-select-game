using UnityEngine;

public class Slot
{
    private Vector2 cords;
    private bool isOccupied;
    private ICarryable carryable;

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

    public Slot(Vector2 cords, bool isOccupied, Vector3 worldPosition)
    {
        this.cords = cords;
        this.isOccupied = isOccupied;
        WorldPosition = worldPosition;
    }
}