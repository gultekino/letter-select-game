using UnityEngine;

public class Slot
{
    private Vector2 cords;
    private bool isOccupied;

    public Vector2 Cords => cords;
    public bool IsOccupied
    {
        get => isOccupied;
        set => isOccupied = value;
    }

    public Slot(Vector2 cords, bool isOccupied)
    {
        this.cords = cords;
        this.isOccupied = isOccupied;
    }
}