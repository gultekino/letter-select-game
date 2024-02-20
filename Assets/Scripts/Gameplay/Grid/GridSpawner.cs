using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSpawner
{
    public List<Slot> SpawnGrid(GridConfiguration gridConfigurationData,GameObject slotPrefab,Transform gridSlotsParent, SlotLocation slotLocation)
    {
        List<Slot> regularSlots = new List<Slot>();
        for (int column = 0; column < gridConfigurationData.Column; column++)
        {
            for (int row = 0; row < gridConfigurationData.Row; row++)
            {
                var pos =gridConfigurationData.GridPosition + new Vector2(row * 1, column * 1);
                regularSlots.Add(new Slot(new Vector2(row, column), false,pos,slotLocation));
                var gridSlot =  GameObject.Instantiate(slotPrefab, gridSlotsParent);
                gridSlot.transform.position = pos;
            }
        }

        return regularSlots;
    }
}
