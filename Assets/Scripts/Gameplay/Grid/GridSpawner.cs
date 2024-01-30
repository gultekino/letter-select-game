using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSpawner
{
    public List<Slot> SpawnGrid(GridConfiguration gridConfigurationData,GameObject slotPrefab,Transform gridSlotsParent)
    {
        List<Slot> regularSlots = new List<Slot>();
        for (int row = 0; row < gridConfigurationData.Row; row++)
        {
            for (int column = 0; column < gridConfigurationData.Column; column++)
            {
                regularSlots.Add(new Slot(new Vector2(row, column), false));
                var gridSlot =  GameObject.Instantiate(slotPrefab, gridSlotsParent);
                gridSlot.transform.position = gridConfigurationData.GridPosition + new Vector2(row * 2, column * 2);
            }
        }

        return regularSlots;
    }
}
