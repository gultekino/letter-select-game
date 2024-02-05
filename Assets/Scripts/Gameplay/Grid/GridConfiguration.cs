using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridConfiguration
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private Vector2 gridPosition;
    
    public int Row
    {
        get => row;
        set => row = value;
    }

    public int Column => column;
    public Vector2 GridPosition => gridPosition;
    public Vector2 WorldPosition => new Vector2(gridPosition.x * 2, gridPosition.y * 2);
    
    public GridConfiguration(int row, int column, Vector2 gridPosition)
    {
        this.row = row;
        this.column = column;
        this.gridPosition = gridPosition;
    }
}
