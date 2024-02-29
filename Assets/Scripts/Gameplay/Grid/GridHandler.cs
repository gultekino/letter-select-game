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
        var letterFrequency = GoalManager.Instance.GetLetterFrequencies().
            Select(frequency => new LetterFrequency(frequency.Letter,frequency.Frequency) ).ToList(); //copy of the list
        
        letterFrequency.ForEach(obj => obj.Frequency += Random.Range(0, 2));
        
        foreach (var letter in letterFrequency)
        {
            for (int i = 0; i < letter.Frequency; i++)
            {
                var slot = GetEmptySlot();
                if (slot == null){
                    Debug.Log("No empty slot left!");
                    break;
                }
                var letterCarrier = LetterManager.Instance.SpawnLetterCarrier(slot, letter.Letter);
                slot.CarryItem(letterCarrier);
            }
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
        Slot emptySlot = null;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].IsOccupied && emptySlot != null)
            {
                emptySlot.ChangeLetterWithAnotherSlot(Slots[i]);
                AlignLettersToRight();
                return;
            }

            if (!Slots[i].IsOccupied)
            {
                emptySlot = Slots[i];
            }
        }
    }

    public void AlignLettersToDown(Slot emptiedSlot)
    {
        int indexOfEmptiedSlot = Slots.IndexOf(emptiedSlot);
        for (int i = indexOfEmptiedSlot; i < Slots.Count; i+=gridConfiguration.Row)
        {
            if (i + gridConfiguration.Row < Slots.Count && Slots[i + gridConfiguration.Row].IsOccupied)
            {
                Slots[i].ChangeLetterWithAnotherSlot(Slots[i + gridConfiguration.Row]);
            }
        }
    }
}