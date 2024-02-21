using System.Collections.Generic;
using UnityEngine;

public class TablePiece
{
    public List<Slot> Slots { get; }
    public int LineNumber { get; }
    public Vector2 StartPosition { get; }

    public TablePiece(List<Slot> slots, int lineNumber, Vector2 startPosition)
    {
        Slots = slots;
        LineNumber = lineNumber;
        StartPosition = startPosition;
    }
}