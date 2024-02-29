using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] GridConfiguration gridConfiguration;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotsParent;
    [SerializeField] private SlotLocation slotLocation;

    public List<Slot> Slots { get; private set; } = new List<Slot>();

    public void InitializeGrid(int row = -1)
    {
        if (row!=-1){
            gridConfiguration.Row = row;
            slotsParent = new GameObject("CGridSlotHolder").transform;
        }
        GridSpawner spawner = new GridSpawner();
        Slots = spawner.SpawnGrid(gridConfiguration, slotPrefab ,slotsParent,slotLocation);
    }
    
    public Slot GetSlot(Vector2 gridPosition)
    {
        int index = GetIndex(gridPosition);
        return Slots[index];
    }

    private int GetIndex(Vector2 gridPosition)
    {
        return gridConfiguration.Row * (int)gridPosition.x + (int)gridPosition.y;
    }

    public void FillGridWithLetterCarriers()
    {
        foreach (var slot in Slots)
        {
            var letterCarrier = LetterManager.Instance.SpawnLetterCarrier(slot);
            slot.CarryItem(letterCarrier);
        }
    }

    public Slot GetEmptySlot()
    {
        foreach (var slot in Slots)
        {
            if (!slot.IsOccupied)
                return slot;
        }

        return null;
    }
    
    public int GetEmptySlotsCount()
    {
        return Slots.Count(slot => !slot.IsOccupied);
    }

    public void DestroySlotsParent()
    {
        Destroy(slotsParent.gameObject);
    }

    public void AlignLettersToRight()
    {
        Slot emptySlot=null;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].IsOccupied && emptySlot != null)
            {
                emptySlot.ChangeLetterWithAnotherSlot(Slots[i]);
                emptySlot= Slots[i];
                continue;
            }
            if (!Slots[i].IsOccupied)
            {
                emptySlot = Slots[i];
            }
        }
    }
}