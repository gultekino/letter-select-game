using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] GridConfiguration gridConfiguration;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotsParent;
    [SerializeField] private SlotLocation slotLocation;

    private List<Slot> slots = new List<Slot>();
    
    public void InitializeGrid(int row = -1)
    {
        if (row!=-1){
            gridConfiguration.Row = row;
            slotsParent = new GameObject("CGridSlotHolder").transform;
        }
        GridSpawner spawner = new GridSpawner();
        slots = spawner.SpawnGrid(gridConfiguration, slotPrefab ,slotsParent,slotLocation);
    }
    
    public Slot GetSlot(Vector2 gridPosition)
    {
        int index = GetIndex(gridPosition);
        return slots[index];
    }

    private int GetIndex(Vector2 gridPosition)
    {
        return gridConfiguration.Row * (int)gridPosition.x + (int)gridPosition.y;
    }

    public void FillGridWithLetterCarriers()
    {
        foreach (var slot in slots)
        {
            var letterCarrier = LetterManager.Instance.SpawnLetterCarrier(slot);
            slot.CarryItem(letterCarrier);
        }
    }

    public Slot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsOccupied)
                return slot;
        }

        return null;
    }
    
    public int GetEmptySlotsCount()
    {
        return slots.Count(slot => !slot.IsOccupied);
    }

    public void ClearGrid()
    {
    }

    public void MoveLettersToPos()
    {
        slotsParent.transform.position = new Vector3(0, -20, 0);
        foreach (var slot in slots)
        {
            slot.Carryable.GetCarried(slot);
        }
    }
}