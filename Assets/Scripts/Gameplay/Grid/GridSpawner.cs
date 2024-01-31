using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSpawner
{
    public List<Slot> SpawnGrid(GridConfiguration gridConfigurationData,GameObject slotPrefab,Transform gridSlotsParent)
    {
        List<Slot> regularSlots = new List<Slot>();
        for (int column = 0; column < gridConfigurationData.Column; column++)
        {
            for (int row = 0; row < gridConfigurationData.Row; row++)
            {
                var pos =gridConfigurationData.GridPosition + new Vector2(row * 2, column * 2);
                regularSlots.Add(new Slot(new Vector2(row, column), false,pos));
                var gridSlot =  GameObject.Instantiate(slotPrefab, gridSlotsParent);
                gridSlot.transform.position = pos;
            }
        }

        return regularSlots;
    }
}
