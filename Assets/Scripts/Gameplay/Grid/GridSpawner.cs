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
                float x = row * gridConfigurationData.OffsetBetweenSlots.x - (gridConfigurationData.Row-1) * gridConfigurationData.OffsetBetweenSlots.x / 2;
                float y = column * gridConfigurationData.OffsetBetweenSlots.y - (gridConfigurationData.Column-1) * gridConfigurationData.OffsetBetweenSlots.y / 2;
                Vector2 move = gridConfigurationData.GridCenterPosition - new Vector2(x, y);
                regularSlots.Add(new Slot(new Vector2(row, column), false,move,slotLocation));
                var gridSlot =  Object.Instantiate(slotPrefab, gridSlotsParent);
                gridSlot.transform.position = move;
                
            }
        }
        return regularSlots;
    }
}
