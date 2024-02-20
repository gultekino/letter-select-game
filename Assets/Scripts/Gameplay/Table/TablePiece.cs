using System.Collections.Generic;
using UnityEngine;

public class TablePiece
{
    public int wordIndex;
    public int lineIndex;
    public Vector2 startLocation;
    public List<Slot> tableSlots;

    public TablePiece(int wordIndex, int lineIndex, Vector2 startLocation, List<Slot> tableSlots)
    {
        this.wordIndex = wordIndex;
        this.lineIndex = lineIndex;
        this.startLocation = startLocation;
        this.tableSlots = tableSlots;
    }
}