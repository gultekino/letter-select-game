using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] GridConfiguration gridConfiguration;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotsParent;

    private List<Slot> slots = new List<Slot>();
    public void InitializeGrid()
    {
        GridSpawner spawner = new GridSpawner();
        slots = spawner.SpawnGrid(gridConfiguration, slotPrefab ,slotsParent);
    }
    
    public Slot GetSlot(Vector2 gridPosition)
    {
        int index = GetIndex(gridPosition);
        return slots[index];
    }
    
    public void SetOccupation(Vector2 gridPosition, bool isOccupied)
    {
        int index = GetIndex(gridPosition);
        slots[index].IsOccupied = isOccupied;
    }

    private int GetIndex(Vector2 gridPosition)
    {
        return gridConfiguration.Row * (int)gridPosition.x + (int)gridPosition.y;
    }
}